using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsAndUtilModsController : MonoBehaviour
{

    [SerializeField] private GameObject _turret;
    [SerializeField] private Transform _cameraAimLookAt;
    public WeaponMod _frontWeaponMod;
    public WeaponMod _topWeaponMod;
    public Modification _backModification;

    //[SerializeField] private float _rotateSpeed;
    //[SerializeField] private float _pitchSpeed;
    //private const string MOUSEX = "Mouse X";
    //private const string MOUSEY = "Mouse Y";
    //private float turretXAngle;
    //private float turretYAngle;

    private void Start()
    {

        GameObject[] hardpoints = GetComponent<Hellrider>().PresentHardpoints();

        //Connect Primary Weapon
        _frontWeaponMod = hardpoints[0].GetComponentInChildren<WeaponMod>();
        
        //connect turret and weapon
        _turret = hardpoints[1];
        _topWeaponMod = hardpoints[1].GetComponentInChildren<WeaponMod>();

        //connect util mod
        _backModification = hardpoints[2].GetComponentInChildren<Modification>();

        _cameraAimLookAt = Camera.main.gameObject.transform.GetChild(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        AimAtTargetPosition(_cameraAimLookAt.position);
    }

    public void StartFiringPrimaryWeapons()
    {
        if(_frontWeaponMod && _frontWeaponMod.canFire)
            _frontWeaponMod.StartFiring();
    }

    public void StopFiringPrimaryWeapons()
    {
        if(_frontWeaponMod)
            _frontWeaponMod.StopFiring();
    }

    public void StartFiringTurretWeapons()
    {
        if (_topWeaponMod && _topWeaponMod.canFire)
            _topWeaponMod.StartFiring();
    }

    public void StopFiringTurretWeapons()
    {
        if (_topWeaponMod)
            _topWeaponMod.StopFiring();
    }

    public void ActivateUtilityMod()
    {
        if (_backModification)
            Debug.Log("Utility Activated.");
    }

    //private void CalculateTargetPosition(Vector3 hit)
    //{
    //    Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //    float yTotal = hit.y - mouseScreenToWorld.y;
    //    float newYTotal = yTotal - (hit.y - _turret.transform.position.y);
    //    float factor = newYTotal / yTotal;

    //    Vector3 targetPos = mouseScreenToWorld + ((hit - mouseScreenToWorld) * factor);

    //    _turret.transform.LookAt(targetPos);

    //}

    private void AimAtTargetPosition(Vector3 targetPos)
    {
        _turret.transform.LookAt(targetPos);
    }

}
