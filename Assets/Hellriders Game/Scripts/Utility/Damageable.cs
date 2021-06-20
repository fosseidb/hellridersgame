using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    protected float _health;
    protected float _armor;

    public virtual void TakeDamage(float damage)
    {
        Debug.Log("Taking " + damage + " damage!");
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
        Debug.Log("YOU DIE!");
    }
}
