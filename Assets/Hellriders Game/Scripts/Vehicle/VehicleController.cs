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
    [Range(0, 1)] [SerializeField] private float _steerHelper; // 0 is raw physics, 1 the car will grip in the direction it is facing
    [Range(0, 1)] [SerializeField] private float _tractionControl; // 0 is no traction control, 1 is full interference
    [SerializeField] private float _fullTorqueOverAllWheels;
    [SerializeField] private float _downforce = 100f;
    [SerializeField] private float _topspeed = 200;
    [SerializeField] private float _brakeTorque;
    [SerializeField] private float _reverseTorque;
    [SerializeField] private float _maxHandbrakeTorque;
    [SerializeField] private static int NoOfGears = 5;
    [SerializeField] private float _revRangeBoundary = 1f;
    [SerializeField] private float _slipLimit;

    private Quaternion[] _wheelMeshLocalRotations;
    private float _steerAngle;
    private int _gearNum;
    private float _gearFactor;
    private float _oldRotation;
    private float _currentTorque;
    private Rigidbody _rigidbody;
    private const float k_ReversingThreshold = 0.01f;

    public float BrakeInput { get; private set; }
    public float AccelInput { get; private set; }
    public float MaxSpeed { get { return _topspeed; } }
    public float CurrentSpeed { get { return _rigidbody.velocity.magnitude * 2.23693629f; } }
    public float CurrentSteerAngle { get { return _steerAngle; } }
    public float Revs { get; private set; }
    public int GearNum { get { return _gearNum; } }

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
        _currentTorque = _fullTorqueOverAllWheels - (_tractionControl * _fullTorqueOverAllWheels);
    }


    public void Move(float steering, float accel, float footbrake, bool handbrake)
    {
        UpdateWheelVisuals();

        //clamp input values
        steering = Mathf.Clamp(steering, -1, 1);
        AccelInput = accel = Mathf.Clamp(accel, 0, 1);
        BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
        //handbrake = Mathf.Clamp(handbrake, 0, 1);

        HandleSteering(steering);

        SteerHelper();
        ApplyDrive(accel, footbrake);
        CapSpeed();

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

        AddDownForce();
        //CheckForWheelSpein()
        TractionControl();
    }

    private void SteerHelper()
    {
        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelhit;
            _wheelColliders[i].GetGroundHit(out wheelhit);
            if (wheelhit.normal == Vector3.zero)
                return; // wheels arent on the ground so dont realign the rigidbody velocity
        }

        // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
        if (Mathf.Abs(_oldRotation - transform.eulerAngles.y) < 10f)
        {
            var turnadjust = (transform.eulerAngles.y - _oldRotation) * _steerHelper;
            Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
            _rigidbody.velocity = velRotation * _rigidbody.velocity;
        }
        _oldRotation = transform.eulerAngles.y;
    }

    // this is used to add more grip in relation to speed
    private void AddDownForce()
    {
        _wheelColliders[0].attachedRigidbody.AddForce(-transform.up * _downforce *
                                                     _wheelColliders[0].attachedRigidbody.velocity.magnitude);
    }

    ///// <summary>
    ///// Checks if the wheels are spinning and is so does three things
    ///// 1) emits particles
    ///// 2) plays tiure skidding sounds
    ///// 3) leaves skidmarks on the ground
    ///// these effects are controlled through the WheelEffects class
    ///// </summary>
    //private void CheckForWheelSpin()
    //{
    //    // loop through all wheels
    //    for (int i = 0; i < 4; i++)
    //    {
    //        WheelHit wheelHit;
    //        _wheelColliders[i].GetGroundHit(out wheelHit);

    //        // is the tire slipping above the given threshhold
    //        if (Mathf.Abs(wheelHit.forwardSlip) >= _slipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= _slipLimit)
    //        {
    //            _wheelEffects[i].EmitTyreSmoke();

    //            // avoiding all four tires screeching at the same time
    //            // if they do it can lead to some strange audio artefacts
    //            if (!AnySkidSoundPlaying())
    //            {
    //                _wheelEffects[i].PlayAudio();
    //            }
    //            continue;
    //        }

    //        // if it wasnt slipping stop all the audio
    //        if (_wheelEffects[i].PlayingAudio)
    //        {
    //            _wheelEffects[i].StopAudio();
    //        }
    //        // end the trail generation
    //        _wheelEffects[i].EndSkidTrail();
    //    }
    //}

    // crude traction control that reduces the power to wheel if the car is wheel spinning too much
    private void TractionControl()
    {
        WheelHit wheelHit;
        switch (_carDriveType)
        {
            case CarDriveType.FourWheelDrive:
                // loop through all wheels
                for (int i = 0; i < 4; i++)
                {
                    _wheelColliders[i].GetGroundHit(out wheelHit);

                    AdjustTorque(wheelHit.forwardSlip);
                }
                break;

            case CarDriveType.RearWheelDrive:
                _wheelColliders[2].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);

                _wheelColliders[3].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);
                break;

            case CarDriveType.FrontWheelDrive:
                _wheelColliders[0].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);

                _wheelColliders[1].GetGroundHit(out wheelHit);
                AdjustTorque(wheelHit.forwardSlip);
                break;
        }
    }

    private void AdjustTorque(float forwardSlip)
    {
        if (forwardSlip >= _slipLimit && _currentTorque >= 0)
        {
            _currentTorque -= 10 * _tractionControl;
        }
        else
        {
            _currentTorque += 10 * _tractionControl;
            if (_currentTorque > _fullTorqueOverAllWheels)
            {
                _currentTorque = _fullTorqueOverAllWheels;
            }
        }
    }


    //private bool AnySkidSoundPlaying()
    //{
    //    for (int i = 0; i < 4; i++)
    //    {
    //        if (_wheelEffects[i].PlayingAudio)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

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
        if (speed > _topspeed)
            _rigidbody.velocity = (_topspeed / 3.6f) * _rigidbody.velocity.normalized;
    }

    ///Gears and revs for audio purposes only
    private void GearChanging()
    {
        float f = Mathf.Abs(CurrentSpeed / MaxSpeed);
        float upgearlimit = (1 / (float)NoOfGears) * (_gearNum + 1);
        float downgearlimit = (1 / (float)NoOfGears) * _gearNum;

        if (_gearNum > 0 && f < downgearlimit)
        {
            _gearNum--;
        }

        if (f > upgearlimit && (_gearNum < (NoOfGears - 1)))
        {
            _gearNum++;
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
        var targetGearFactor = Mathf.InverseLerp(f * _gearNum, f * (_gearNum + 1), Mathf.Abs(CurrentSpeed / MaxSpeed));
        _gearFactor = Mathf.Lerp(_gearFactor, targetGearFactor, Time.deltaTime * 5f);
    }


    private void CalculateRevs()
    {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        CalculateGearFactor();
        var gearNumFactor = _gearNum / (float)NoOfGears;
        var revsRangeMin = ULerp(0f, _revRangeBoundary, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(_revRangeBoundary, 1f, gearNumFactor);
        Revs = ULerp(revsRangeMin, revsRangeMax, _gearFactor);
    }

}
