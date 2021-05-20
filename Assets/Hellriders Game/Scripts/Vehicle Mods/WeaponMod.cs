using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMod : Modification
{
    public bool canFire = false;
    public bool isFiring = false;

    public override void Activate()
    {
        throw new System.NotImplementedException();
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
}
