using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modification : MonoBehaviour
{
    private int _uses;

    public int Uses { get => _uses; set => _uses = value; }

    public abstract void Activate();


}
