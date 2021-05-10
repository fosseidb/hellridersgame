using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInStage : IState
{
    private RaceManager _rm;

    public float introCountdownTimer = 10f;
    public float timeOutTimer = 30f;

    public LoadInStage(RaceManager rm)
    {
        _rm = rm;
    }

    public void OnEnter()
    {

        //set mout lock
        _rm.SetMouseLock(true);

        //check connections to server?
        Debug.Log("Checking connections...");
        _rm._RGUIC.loadInPanel.SetActive(true);
        _rm._RGUIC.levelName.text = "Welcome to "+ _rm.raceLevelName + "!";


        _rm._RGUIC.countDownPanel.SetActive(false);
        _rm._RGUIC.racePanel.SetActive(false);
        _rm._RGUIC.finishPanel.SetActive(false);

        //Set cameramode
        _rm._CMRC.OnLoadIn();
    }

    public void OnExit()
    {
        Debug.Log("All players loaded, intro done and ready to count down!");
        _rm._RGUIC.loadInPanel.SetActive(false);
    }

    public void Tick()
    {
        //check timeout for loading players.
        Debug.Log("Waiting for all players to load....");

        introCountdownTimer -= Time.deltaTime;
        timeOutTimer -= Time.deltaTime;
    }
}