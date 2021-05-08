using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Input.mousePosition;
    }
}
