using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaceGUIController : MonoBehaviour
{

    public TMP_Text _raceTimer;
    public TMP_Text _countDownTimer;
    public GameObject _countDownPanel;

    public void UpdateRaceTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = (timeToDisplay % 1) * 1000;

        _raceTimer.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
    }

    public void UpdateCountdownTime(float timeToDisplay)
    {
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        _countDownTimer.text = seconds.ToString();
    }
}
