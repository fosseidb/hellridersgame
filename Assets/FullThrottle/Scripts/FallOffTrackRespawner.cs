using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffTrackRespawner : MonoBehaviour
{
    [Range(0.1f,4f)]
    private float _acceptableFallingTime = 2f;
    private float fallingTimer;
    private LayerMask _layerMask;

    [SerializeField] private float acceptableDistToRoad = 1f;

    public Transform _initialRespawnPoint;
    private Transform _respawnPoint;

    [Range(0.1f, 4f)]
    private float _respawnSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        _respawnPoint = _initialRespawnPoint;
        _layerMask = LayerMask.GetMask("Road");

    }

    // Update is called once per frame
    void Update()
    {
        fallingTimer = IsGrounded() ? 0f : fallingTimer += Time.deltaTime;

        if (fallingTimer >= _acceptableFallingTime)
        {
            RespawnFromFall();
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, -Vector3.up, acceptableDistToRoad, _layerMask))
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.1f, -Vector3.up, Color.green);
            return true;
        }
        return false;
    }

    private void RespawnFromFall()
    {
        transform.position = _respawnPoint.position+Vector3.up*0.2f;
        transform.rotation = _respawnPoint.rotation;
        GetComponent<Rigidbody>().velocity = new Vector3(1,1,0);
        Camera.main.transform.rotation = _respawnPoint.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("RespawnCheckpoint"))
        {
            _respawnPoint = other.transform;
        }
    }
}