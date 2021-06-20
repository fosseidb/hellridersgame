using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    public List<GameObject> pooledCannonballs;
    public List<GameObject> pooledPlasmaPulses;
    public GameObject[] bulletPrefabs;
    public int maxAmountOfBullets;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject temp;

        //cannonballs
        pooledCannonballs = new List<GameObject>();
        for (int i = 0; i < maxAmountOfBullets; i++)
        {
            temp = Instantiate(bulletPrefabs[0],transform);
            temp.SetActive(false);
            pooledCannonballs.Add(temp);

        }

        //plasma pulses
        pooledPlasmaPulses = new List<GameObject>();
        for (int i = 0; i < maxAmountOfBullets; i++)
        {
            temp = Instantiate(bulletPrefabs[1], transform);
            temp.SetActive(false);
            pooledPlasmaPulses.Add(temp);

        }
    }

    public GameObject GetPooledCannonball()
    {
        for(int i =0; i<maxAmountOfBullets; i++)
        {
            if (!pooledCannonballs[i].activeInHierarchy)
            {
                return pooledCannonballs[i];
            }
        }
        return null;
    }

    public GameObject GetPooledPlasmaPulse()
    {
        for (int i = 0; i < maxAmountOfBullets; i++)
        {
            if (!pooledPlasmaPulses[i].activeInHierarchy)
            {
                return pooledPlasmaPulses[i];
            }
        }
        return null;
    }
}
