using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaceGUIController : MonoBehaviour
{

    public TMP_Text raceTimer;
    public TMP_Text countDownTimer;
    public TMP_Text levelName;

    [Header("Panels")]
    public GameObject loadInPanel;
    public GameObject countDownPanel;
    public GameObject racePanel;
    public GameObject finishPanel;

    [Header("Dashboard")]
    private Hellrider _hellrider;
    public TMP_Text revText;
    public TMP_Text currentSpeedText;
    public TMP_Text gearNumText;

    private void FixedUpdate()
    {
        if (_hellrider == null)
            return;

        currentSpeedText.text = Mathf.Floor(_hellrider.GetComponent<VehicleController>().CurrentSpeed).ToString() + " km/h";
        revText.text = Mathf.Floor(_hellrider.GetComponent<VehicleController>().Revs*1000).ToString() + " rpm";
        gearNumText.text = "["+ _hellrider.GetComponent<VehicleController>().GearNum.ToString() + "]";

    }
    public void UpdateRaceTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = (timeToDisplay % 1) * 1000;

        raceTimer.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
    }

    public void UpdateCountdownTime(float timeToDisplay)
    {
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        countDownTimer.text = seconds.ToString();
    }

    internal void HookUpGUI(Hellrider hellrider)
    {
        _hellrider = hellrider;
    }

    public void SetUniquePanel(int i)
    {
        loadInPanel.SetActive(i == 0);
        countDownPanel.SetActive(i == 1);
        racePanel.SetActive(i == 2);
        finishPanel.SetActive(i == 3);
    }
}
