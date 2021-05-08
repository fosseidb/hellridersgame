using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (VehicleController))]
public class VehicleUserController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private VehicleController _vehicleController;

    private Quaternion camRotation;

    Camera _mainCamera;

    private void Awake()
    {
        _vehicleController = GetComponent<VehicleController>();
        camRotation = Camera.main.transform.localRotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
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
}

