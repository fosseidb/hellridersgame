using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask includeLayers;
    [SerializeField] private float _lifetime = 5f;
    [SerializeField]private float _bulletDamageAmount = 5f;
    [SerializeField] private float _movementSpeed = 20f;

    [SerializeField] private ParticleSystem _trail;
    [SerializeField] private GameObject _bulletMesh; 

    private float _lifeLeft;
    private float _actualMovementSpeed;

    private void OnEnable()
    {
        _lifeLeft = _lifetime;
        _trail.Play();
        _bulletMesh.SetActive(true);
        _actualMovementSpeed = _movementSpeed;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        _lifeLeft -= Time.deltaTime;

        if (_lifeLeft <= 0)
            Explode(null);

        transform.Translate(Vector3.forward * Time.deltaTime * _actualMovementSpeed);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Explode(other.gameObject);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        Explode(collision.gameObject);
    }

    private void Explode(GameObject other)
    {
        Debug.Log("Booom!");
        if(other != null)
        {
            if(other.GetComponent<Damageable>()!=null)
                other.GetComponent<Damageable>().TakeDamage(_bulletDamageAmount);

            // clean up bullet
            StartCoroutine("CleanUp");
        }
    }

    IEnumerator CleanUp()
    {
        _trail.Stop();
        _bulletMesh.SetActive(false);
        _actualMovementSpeed = 0;

        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
