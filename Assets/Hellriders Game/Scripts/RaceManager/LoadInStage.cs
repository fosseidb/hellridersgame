using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInStage : IState
{
    private RaceManager _rm;
    public float _introCountdownTimer = 10f;

    public LoadInStage(RaceManager rm)
    {
        _rm = rm;
    }

    public void OnEnter()
    {
        //check connections to server?
        Debug.Log("Checking connections...");
        _rm._RGUIC._countDownPanel.SetActive(false);
        _rm._RGUIC._racePanel.SetActive(false);
        
        //Set cameramode
        _rm._CMRC.OnLoadIn();
    }

    public void OnExit()
    {
        Debug.Log("All players loaded");
        _rm._RGUIC._countDownPanel.SetActive(true);
    }

    public void Tick()
    {
        //check timeout for loading players.
        Debug.Log("Waiting for all players to load....");

        _introCountdownTimer -= Time.deltaTime;

    }
}