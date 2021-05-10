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
        //Set correct panel
        _rm._RGUIC.racePanel.gameObject.SetActive(true);

        //clear timer
        _rm.raceTimer = 0f;
    }

    public void OnExit()
    {
        //set correct panel
        _rm._RGUIC.racePanel.gameObject.SetActive(false);
    }

    public void Tick()
    {
        _rm.raceTimer += Time.deltaTime;
        UpdateUITimer();
    }

    public void UpdateUITimer()
    {
        _rm._RGUIC.UpdateRaceTime(_raceTimer);
    }
}
