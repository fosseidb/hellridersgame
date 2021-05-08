using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FinishLine : MonoBehaviour
{

    // Define delegate
    public delegate void PlayerCrossedFinishLine(GameObject hellrider);
    public event PlayerCrossedFinishLine OnCrossFinishLine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnCrossFinishLine?.Invoke(other.gameObject);
        }
    }
}
