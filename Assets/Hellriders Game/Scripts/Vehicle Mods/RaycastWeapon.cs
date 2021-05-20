using System.Collections;
using UnityEngine;

namespace Assets.Hellriders_Game.Scripts.Vehicle_Mods
{
    public class RaycastWeapon : MonoBehaviour
    {
        public bool isFiring = false;

        public void StartFiring()
        {
            isFiring = true;
        }

        public void StopFiring()
        {
            isFiring = false;
        }
    }
}