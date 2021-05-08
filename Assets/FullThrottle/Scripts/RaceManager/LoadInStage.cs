using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInStage : IState
{
    private RaceManager _rm;

    public LoadInStage(RaceManager rm)
    {
        _rm = rm;
    }

    public void OnEnter()
    {
        //check connections to server?
        Debug.Log("Checking connections...");
    }

    public void OnExit()
    {
        Debug.Log("All players loaded");
        
    }

    public void Tick()
    {
        //check timeout for loading players.
        Debug.Log("Waiting for all players to load....");
    }
}
