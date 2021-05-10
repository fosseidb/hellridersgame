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
        _rm._RGUIC.finishPanel.SetActive(true);
    }

    public void OnExit()
    {
        _rm._RGUIC.finishPanel.SetActive(false);
    }

    public void Tick()
    {
        
    }
}
