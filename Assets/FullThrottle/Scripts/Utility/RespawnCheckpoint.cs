using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour
{

    [SerializeField] private float _heightOffRoad = 1f;

    //private void OnDrawGizmos()
    //{
    //    //Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up)* _heightOffRoad, Color.red);
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(transform.position, transform.position - Vector3.up * _heightOffRoad);
    //}

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
