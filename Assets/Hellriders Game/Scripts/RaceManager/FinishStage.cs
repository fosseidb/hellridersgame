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
        //set mouse lock
        _rm.SetMouseLock(false);

        //Set cameramode
        _rm._cinemachineRaceController.OnFinishRace();

        //show Finish race panel GUI
        _rm._raceGUIController.finishPanel.SetActive(true);
    }

    public void OnExit()
    {
        _rm._raceGUIController.finishPanel.SetActive(false);
    }

    public void Tick()
    {
        
    }
}
