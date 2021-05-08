using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum CarDriveType
{
    FrontWheelDrive,
    RearWheelDrive,
    FourWheelDrive
}

public class VehicleController : MonoBehaviour
{
    [SerializeField] private CarDriveType _carDriveType = CarDriveType.FourWheelDrive;
    [SerializeField] private WheelCollider[] _wheelColliders = new WheelCollider[4]; //frontleft, front right, backleft, back right
    [SerializeField] private GameObject[] _wheelMeshes = new GameObject[4];
    //[SerializeField] private WheelEffects[] m_WheelEffects = new WheelEffects[4];
    [SerializeField] private Vector3 _centreOfMassOffset;
    [SerializeField] private float _maximumSteerAngle;
    [Range(0, 1)] [SerializeField] private float m_TractionControl; // 0 is no traction control, 1 is full interference
    [SerializeField] private float _fullTorqueOverAllWheels;
    [SerializeField] private float m_Topspeed = 200;
    [SerializeField] private float _brakeTorque;
    [SerializeField] private float _reverseTorque;
    [SerializeField] private float _maxHandbrakeTorque;
    [SerializeField] private static int NoOfGears = 5;
    [SerializeField] private float m_RevRangeBoundary = 1f;

    private Quaternion[] _wheelMeshLocalRotations;
    private Rigidbody _rigidbody;
    private float _currentTorque;
    private float _steerAngle;
    private int m_GearNum;
    private float m_GearFactor;

    public float BrakeInput { get; private set; }
    public float AccelInput { get; private set; }
    public float MaxSpeed { get { return m_Topspeed; } }
    public float CurrentSpeed { get { return _rigidbody.velocity.magnitude * 2.23693629f; } }
    public float CurrentSteerAngle { get { return _steerAngle; } }
    public float Revs { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        _wheelMeshLocalRotations = new Quaternion[4];
        for (int i = 0; i < 4; i++)
        {
            _wheelMeshLocalRotations[i] = _wheelMeshes[i].transform.localRotation;
        }
        _wheelColliders[0].attachedRigidbody.centerOfMass = _centreOfMassOffset;

        _maxHandbrakeTorque = float.MaxValue;

        _rigidbody = GetComponent<Rigidbody>();
        _currentTorque = _fullTorqueOverAllWheels - (m_TractionControl * _fullTorqueOverAllWheels);
    }


    public void Move(float steering, float accel, float footbrake, bool handbrake)
    {

        //clamp input values
        steering = Mathf.Clamp(steering, -1, 1);
        AccelInput = accel = Mathf.Clamp(accel, 0, 1);
        BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
        //handbrake = Mathf.Clamp(handbrake, 0, 1);


        ApplyDrive(accel, footbrake);
        CapSpeed();
        HandleSteering(steering);
        UpdateWheelVisuals();

        //Set the handbrake.
        //Assuming that wheels 2 and 3 are the rear wheels.
        if (handbrake)
        {
            var hbTorque = _maxHandbrakeTorque;
            _wheelColliders[2].brakeTorque = hbTorque;
            _wheelColliders[3].brakeTorque = hbTorque;
        }

        CalculateRevs();
        GearChanging();

    }

    private void UpdateWheelVisuals()
    {
        for (int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 position;
            _wheelColliders[i].GetWorldPose(out position, out quat);
            _wheelMeshes[i].transform.position = position;
            _wheelMeshes[i].transform.rotation = quat;
        }
    }

    private void HandleSteering(float steering)
    {
        _steerAngle = steering * _maximumSteerAngle;
        _wheelColliders[0].steerAngle = _steerAngle;
        _wheelColliders[1].steerAngle = _steerAngle;
    }

    private void ApplyDrive(float accel, float footbrake)
    {
        //set torque on wheel colliders to accelerate vehicle.
        float thrustTorque;
        switch (_carDriveType)
        {
            case CarDriveType.FourWheelDrive:
                thrustTorque = accel * (_currentTorque / 4f);
                for(int i = 0; i<4; i++)
                {
                    _wheelColliders[i].motorTorque = thrustTorque;
                }

                break;
        }

        // break or reverse
        for(int i = 0; i <4; i++)
        {
            if(CurrentSpeed>5 && Vector3.Angle(transform.forward, _rigidbody.velocity) < 50f)
            {
                _wheelColliders[i].brakeTorque = _brakeTorque * footbrake;
            }
            else if (footbrake > 0)
            {
                _wheelColliders[i].brakeTorque = 0f;
                _wheelColliders[i].motorTorque = -_reverseTorque * footbrake;
            }
        }
    }

    private void CapSpeed()
    {
        float speed = _rigidbody.velocity.magnitude;

        speed *= 3.6f;
        if (speed > m_Topspeed)
            _rigidbody.velocity = (m_Topspeed / 3.6f) * _rigidbody.velocity.normalized;
    }

    ///Gears and revs for audio purposes only
    private void GearChanging()
    {
        float f = Mathf.Abs(CurrentSpeed / MaxSpeed);
        float upgearlimit = (1 / (float)NoOfGears) * (m_GearNum + 1);
        float downgearlimit = (1 / (float)NoOfGears) * m_GearNum;

        if (m_GearNum > 0 && f < downgearlimit)
        {
            m_GearNum--;
        }

        if (f > upgearlimit && (m_GearNum < (NoOfGears - 1)))
        {
            m_GearNum++;
        }
    }


    // simple function to add a curved bias towards 1 for a value in the 0-1 range
    private static float CurveFactor(float factor)
    {
        return 1 - (1 - factor) * (1 - factor);
    }


    // unclamped version of Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }


    private void CalculateGearFactor()
    {
        float f = (1 / (float)NoOfGears);
        // gear factor is a normalised representation of the current speed within the current gear's range of speeds.
        // We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
        var targetGearFactor = Mathf.InverseLerp(f * m_GearNum, f * (m_GearNum + 1), Mathf.Abs(CurrentSpeed / MaxSpeed));
        m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime * 5f);
    }


    private void CalculateRevs()
    {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        CalculateGearFactor();
        var gearNumFactor = m_GearNum / (float)NoOfGears;
        var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
        Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
    }

}
