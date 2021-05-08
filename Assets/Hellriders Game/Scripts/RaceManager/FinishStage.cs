using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishStage : IState
{
    private RaceManager _rm;


    public FinishStage(RaceManager rm)
    {
        _rm = rm;
    }

    public void OnEnter()
    {
        //Set cameramode
        _rm._CMRC.OnFinishRace();

        //show Finish race panel GUI

    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }
}
