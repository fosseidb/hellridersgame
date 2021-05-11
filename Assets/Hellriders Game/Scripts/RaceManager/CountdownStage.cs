using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownStage : IState
{
    private RaceManager _rm;

    private const float TIMETOCOUNTDOWN = 3f;
    public float _raceCountdownTimer;

    public CountdownStage(RaceManager rm)
    {
        _rm = rm;
    }

    public void OnEnter()
    {
       
        // set correct panel
        _rm._raceGUIController.countDownPanel.SetActive(true);

        //Set cameramode
        _rm._cinemachineRaceController.OnRaceStart();

        //Setting timer to 3 sec
        _raceCountdownTimer = TIMETOCOUNTDOWN;
    }

    public void OnExit()
    {

        //close panel
        _rm._raceGUIController.countDownPanel.SetActive(false);
    }

    public void Tick()
    {
        _raceCountdownTimer -= Time.deltaTime;
        UpdateCountdownTimer();
    }

    public void UpdateCountdownTimer()
    {
        _rm._raceGUIController.UpdateCountdownTime(_raceCountdownTimer);
        if(_raceCountdownTimer <= 1f) _rm._raceGUIController.racePanel.SetActive(true);
    }
}
