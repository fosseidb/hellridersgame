using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineMainMenuController : MonoBehaviour
{
    private Animator animator;

    private const string PORTAL = "Portal";
    private const string HELLRIDER = "HellriderFront";
    private const string HELLRIDERB = "HellriderBack";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnClickMainMenu()
    {
        animator.Play(PORTAL);
    }

    public void OnClickHellrider()
    {
        animator.Play(HELLRIDER);
    }

    public void OnClickHellriderBack()
    {
        animator.Play(HELLRIDERB);
    }
}
