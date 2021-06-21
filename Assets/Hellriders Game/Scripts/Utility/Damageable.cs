using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] protected float _health;
    [SerializeField] protected float _armor;
    [SerializeField] protected ParticleSystem _explosionParticle;
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _explosionClip;

    public virtual void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " is taking " + damage + " damage!");
        if (_armor > 0)
        {
            if (damage < _armor)
            {
                _armor -= damage;
                return;
            }
            else
            {
                damage = damage - _armor;
                _armor = 0f;
            }
        }
        if(_health > damage)
        {
            _health -= damage;
        }
        else
        {
            _health = 0f;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " DIES!");
        if (gameObject.tag != "Player")
            StartCoroutine("Explode");
    }

    IEnumerator Explode()
    {
        if(_explosionParticle != null)
            _explosionParticle.Play();

        if (_audioSource != null)
        {
            _audioSource.clip = _explosionClip;
            _audioSource.Play();
        }

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
