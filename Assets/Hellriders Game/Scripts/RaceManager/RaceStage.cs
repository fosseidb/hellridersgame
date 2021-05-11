using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStage : IState
{
    private RaceManager _rm;
    //private float _raceTimer;
    public RaceStage(RaceManager rm)
    {
        _rm = rm;
    }
    public void OnEnter()
    {
        //Set correct panel
        //_rm._raceGUIController.racePanel.gameObject.SetActive(true);
        _rm._raceGUIController.SetUniquePanel(2);

        //clear timer
        _rm.raceTimer = 0f;

        // give all drivers user acces
        _rm.GiveHellridersUserControlAccess(_rm._hellrider, true);
    }

    public void OnExit()
    {
        //set correct panel
        _rm._raceGUIController.racePanel.gameObject.SetActive(false);
    }

    public void Tick()
    {
        _rm.raceTimer += Time.deltaTime;
        UpdateUITimer();
    }

    public void UpdateUITimer()
    {
        _rm._raceGUIController.UpdateRaceTime(_rm.raceTimer);
    }
}
