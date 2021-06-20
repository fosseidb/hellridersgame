using System;
using UnityEngine;

public class EnemyTurret : Damageable
{

    public int rateOfFire;

    [SerializeField] GameObject _turret;
    [SerializeField] ParticleSystem _cannonParticleSystem;
    Transform _target;

    private bool _isLockedOnTarget = false;
    private float _shootTimer;

    // Update is called once per frame
    void Update()
    {
        if (_isLockedOnTarget)
        {
            _turret.transform.LookAt(_target);
            if (_shootTimer >= 1f / rateOfFire) 
                Shoot();
            _shootTimer += Time.deltaTime;
        }
    }

    private void Shoot()
    {
        Debug.Log("Spawning bullet!");
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledBullet();
        if(bullet != null)
        {
            bullet.transform.position = _turret.transform.position + _turret.transform.forward;
            bullet.transform.rotation = _turret.transform.rotation;
            bullet.SetActive(true);
            //_cannonParticleSystem.Play();
        }

        _shootTimer = 0f;
            
    }

    public void OnTriggerEnter(Collider other)
    {
        if (_isLockedOnTarget)
            return;

        if(other.tag == "Player")
        {
            _target = other.transform;
            _isLockedOnTarget = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.transform == _target)
        {
            _isLockedOnTarget = false;
            _shootTimer = 0f;
        }
    }
}
