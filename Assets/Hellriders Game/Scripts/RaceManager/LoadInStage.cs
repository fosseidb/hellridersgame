using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInStage : IState
{
    private RaceManager _rm;

    public float introCountdownTimer = 10f;
    public float timeOutTimer = 30f;

    public LoadInStage(RaceManager rm, float introTimer)
    {
        _rm = rm;
        introCountdownTimer = introTimer;
    }

    public void OnEnter()
    {

        //set mout lock
        _rm.SetMouseLock(true);

        //check connections to server?
        Debug.Log("Checking connections...");
        //_rm._raceGUIController.loadInPanel.SetActive(true);
        _rm._raceGUIController.SetUniquePanel(0);
        _rm._raceGUIController.levelName.text = "Welcome to "+ _rm.raceLevelName + "!";


        //_rm._raceGUIController.countDownPanel.SetActive(false);
        //_rm._raceGUIController.racePanel.SetActive(false);
        //_rm._raceGUIController.finishPanel.SetActive(false);

        //Set cameramode
        _rm._cinemachineRaceController.OnLoadIn();
    }

    public void OnExit()
    {
        Debug.Log("All players loaded, intro done and ready to count down!");
        _rm._raceGUIController.loadInPanel.SetActive(false);
    }

    public void Tick()
    {
        //check timeout for loading players.
        Debug.Log("Waiting for all players to load....");

        introCountdownTimer -= Time.deltaTime;
        timeOutTimer -= Time.deltaTime;
    }
}