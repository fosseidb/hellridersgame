using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMod : Modification
{
    public bool canFire = false;
    public bool isFiring = false;
    public float rateOfFire=1f;

    [SerializeField] private Transform _muzzle;

    private float _shootTimer;


    public override void Activate()
    {
        Debug.Log("Not activatable.");
    }

    public void StartFiring()
    {
        Debug.Log("Firing");
        isFiring = true;
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    private void Update()
    {
        _shootTimer += Time.deltaTime;
        if (isFiring && _shootTimer >= 1f / rateOfFire)
            Shoot();
    }

    private void Shoot()
    {
        Debug.Log("Spawning Pulse!");
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledPlasmaPulse();
        if (bullet != null)
        {
            bullet.transform.position = _muzzle.position + _muzzle.forward;
            bullet.transform.rotation = _muzzle.rotation;
            bullet.SetActive(true);
        }

        _shootTimer = 0f;

    }
}
