using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffTrackRespawner : MonoBehaviour
{
    [Range(0.1f,4f)]
    private float _acceptableFallingTime = 3f;
    private float fallingTimer;
    private LayerMask _layerMask;

    [SerializeField] private float acceptableDistToRoad = 1f;

    public Transform _initialRespawnPoint;
    private Transform _respawnPoint;

    public delegate void MilestoneCrossedEvent(Player player, int milestoneID);
    public event MilestoneCrossedEvent OnMilestonePassed;

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
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, -Vector3.up, acceptableDistToRoad, _layerMask))
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.1f, -Vector3.up, Color.green);
            return true;
        }
        return false;
    }

    public void RespawnFromFall()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        transform.position = _respawnPoint.position;
        transform.rotation = _respawnPoint.rotation;
        rb.isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("RespawnCheckpoint"))
        {
            _respawnPoint = other.transform;
            OnMilestonePassed?.Invoke(GetComponent<Player>(), other.GetComponent<RaceTrackMilestone>().milestoneID);
        }
    }
}