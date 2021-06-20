using System;
using UnityEngine;

public class EnemyTurret : Damageable
{

    public int rateOfFire;

    [SerializeField] GameObject _sensor;
    [SerializeField] GameObject _turret;
    [SerializeField] ParticleSystem _cannonParticleSystem;
    public Transform _target;

    public bool _isLockedOnTarget = false;
    public float _shootTimer;

    private void Awake()
    {
        //import health and armor data
        _health = 20f;
        _armor = 0f;
    }

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
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledCannonball();
        if(bullet != null)
        {
            bullet.transform.position = _turret.transform.position + _turret.transform.forward;
            bullet.transform.rotation = _turret.transform.rotation;
            bullet.SetActive(true);
            //_cannonParticleSystem.Play();
        }

        _shootTimer = 0f;
            
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
}
