using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FinishLine : MonoBehaviour
{

    // Define delegate
    public delegate void PlayerCrossedFinishLine(Player player);
    public event PlayerCrossedFinishLine OnCrossFinishLine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnCrossFinishLine?.Invoke(other.GetComponent<Player>());
        }
    }
}
