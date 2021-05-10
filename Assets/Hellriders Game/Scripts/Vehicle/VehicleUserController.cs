using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (VehicleController))]
public class VehicleUserController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private bool _userControlEngaged = false;

    private VehicleController _vehicleController;

    private Quaternion camRotation;
    Camera _mainCamera;

    private void Awake()
    {
        //get the vehicle controller
        _vehicleController = GetComponent<VehicleController>();

        //connect camera
        camRotation = Camera.main.transform.localRotation;
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        if (!_userControlEngaged)
            return;

        // pass the input to the vehicle! Check Standard Assets pack for MultiInputManager script.
        float h = Input.GetAxis(HORIZONTAL);
        float v = Input.GetAxis(VERTICAL);
#if !MOBILE_INPUT
        bool handbrake = Input.GetKey(KeyCode.Space);
        _vehicleController.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
    }

    public void GiveHellriderUserControllAccess(bool access)
    {
        _userControlEngaged = access;
    }
}

