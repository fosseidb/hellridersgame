using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour
{

    [SerializeField] private float _heightOffRoad = 1f;

    private bool IsGrounded
    {
        get => Physics.Raycast(GroundRay, _heightOffRoad);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.red : Color.blue;
        Gizmos.DrawRay(transform.position, Vector3.down * _heightOffRoad);
    }
    private Ray GroundRay
    {
        get => new Ray(transform.position, Vector3.down * _heightOffRoad);
    }

}
