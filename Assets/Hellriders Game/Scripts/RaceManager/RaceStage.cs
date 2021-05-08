using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStage : IState
{
    private RaceManager _rm;
    private float _raceTimer;
    public RaceStage(RaceManager rm)
    {
        _rm = rm;
    }
    public void OnEnter()
    {
        //Set cameramode

        _raceTimer = 0f;
        _rm._RGUIC._raceTimer.gameObject.SetActive(true);
    }

    public void OnExit()
    {
        _rm._RGUIC._raceTimer.gameObject.SetActive(false);
    }

    public void Tick()
    {
        _raceTimer += Time.deltaTime;
        UpdateUITimer();
    }

    public void UpdateUITimer()
    {
        _rm._RGUIC.UpdateRaceTime(_raceTimer);
    }
}
