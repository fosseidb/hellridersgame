using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineRaceController : MonoBehaviour
{
    private Animator animator;

    private const string LEVELINTRO = "LevelIntro";
    private const string COUNTDOWN = "Countdown";
    private const string RACING = "Racing";
    private const string FINISH = "Finish";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnLoadIn()
    {
        animator.Play(LEVELINTRO);
    }

    public void OnCountdown()
    {
        animator.Play(COUNTDOWN);
    }

    public void OnRaceStart()
    {
        animator.Play(RACING);
    }

    public void OnFinishRace()
    {
        animator.Play(FINISH);
    }
}
