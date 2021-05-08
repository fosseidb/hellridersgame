using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{

    [SerializeField] private GameObject _turret;
    [SerializeField] private Transform _cameraAimLookAt;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _pitchSpeed;

    private const string MOUSEX = "Mouse X";
    private const string MOUSEY = "Mouse Y";
    private float turretXAngle;
    private float turretYAngle;

    private void Start()
    {
        _turret = GetComponent<Hellrider>().TopHardpoint;
        _cameraAimLookAt = Camera.main.gameObject.transform.GetChild(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        AimAtTargetPosition(_cameraAimLookAt.position);
        //if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
        //{
        //    CalculateTargetPosition(raycastHit.point);
        //} 
    }

    private void CalculateTargetPosition(Vector3 hit)
    {
        Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float yTotal = hit.y - mouseScreenToWorld.y;
        float newYTotal = yTotal - (hit.y - _turret.transform.position.y);
        float factor = newYTotal / yTotal;

        Vector3 targetPos = mouseScreenToWorld + ((hit - mouseScreenToWorld) * factor);

        _turret.transform.LookAt(targetPos);

    }

    private void AimAtTargetPosition(Vector3 targetPos)
    {
        _turret.transform.LookAt(targetPos);
    }
}
