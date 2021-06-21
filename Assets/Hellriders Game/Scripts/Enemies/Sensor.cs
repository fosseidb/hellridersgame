using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    EnemyTurret turret;
    private void Start()
    {
        turret = transform.parent.GetComponent<EnemyTurret>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (turret._isLockedOnTarget)
            return;

        if (other.tag == "Player")
        {
            turret._target = other.transform;
            turret._isLockedOnTarget = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.transform == turret._target)
        {
            turret._isLockedOnTarget = false;
            turret._shootTimer = 0f;
        }
    }
}
