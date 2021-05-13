using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<FallOffTrackRespawner>()?.RespawnFromFall();
    }
}
