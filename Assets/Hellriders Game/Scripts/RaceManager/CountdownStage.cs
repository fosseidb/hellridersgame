using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownStage : IState
{
    private RaceManager _rm;

    public float _raceCountdownTimer = 3f;

    public CountdownStage(RaceManager rm)
    {
        _rm = rm;
    }

    public void OnEnter()
    {
        //Set cameramode
        _rm._CMRC.OnRaceStart();

        Debug.Log("Setting timer to 3 sec.");
        _raceCountdownTimer = 3f;
        _rm._RGUIC._countDownPanel.SetActive(true);
    }

    public void OnExit()
    {
        Debug.Log("Race!");
        _rm._RGUIC._countDownPanel.SetActive(false);
    }

    public void Tick()
    {
        _raceCountdownTimer -= Time.deltaTime;
        UpdateCountdownTimer();
    }

    public void UpdateCountdownTimer()
    {
        _rm._RGUIC.UpdateCountdownTime(_raceCountdownTimer);
        if(_raceCountdownTimer <= 1f) _rm._RGUIC._racePanel.SetActive(true);
    }
}
