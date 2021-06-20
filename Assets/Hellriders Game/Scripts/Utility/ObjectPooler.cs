using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    public List<GameObject> pooledBullets;
    public GameObject bulletPrefab;
    public int maxAmountOfBullets;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pooledBullets = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < maxAmountOfBullets; i++)
        {
            temp = Instantiate(bulletPrefab,transform);
            temp.SetActive(false);
            pooledBullets.Add(temp);

        }
    }

    public GameObject GetPooledBullet()
    {
        for(int i =0; i<maxAmountOfBullets; i++)
        {
            if (!pooledBullets[i].activeInHierarchy)
            {
                return pooledBullets[i];
            }
        }
        return null;
    }
}
