using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Slider healthSlider;
    public Slider armorSlider;

    [Header("FinishRoster")]
    public GameObject rosterContainer;
    public GameObject hellriderPlacingRowPrefab;

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

        //set up health and armor
        _hellrider.HellriderDamageEvent += UpdateHealthArmor;
        healthSlider.maxValue = _hellrider.hellriderData.hellriderHealth;
        armorSlider.maxValue = _hellrider.hellriderData.hellriderArmor;
        UpdateHealthArmor(_hellrider.hellriderData.hellriderHealth, _hellrider.hellriderData.hellriderArmor);
    }

    public void SetUniquePanel(int i)
    {
        loadInPanel.SetActive(i == 0);
        countDownPanel.SetActive(i == 1);
        racePanel.SetActive(i == 2);
        finishPanel.SetActive(i == 3);
    }
    public void UpdateHealthArmor(float health, float armor)
    {
        healthSlider.value = health;
        armorSlider.value = armor;
    }

    internal void UpdateFinishRoster(List<RacePositionData> roster)
    {

        HellriderPlacingRow[] positionRows = rosterContainer.transform.GetComponentsInChildren<HellriderPlacingRow>();

        Debug.Log("positionRows" + positionRows.Length);
        RacePositionData[] currentRoster = roster.ToArray();
        Debug.Log("currentRoster" + currentRoster.Length);

        for (int i=0; i < roster.Count; i++)
        {
            if (positionRows[i] != null)
            {
                positionRows[i].FillIn(
                    currentRoster[i].positionNr,
                    currentRoster[i].hellrider.hellriderData.hellriderIcon,
                    currentRoster[i].playerName,
                    ConvertToRaceTime(currentRoster[i].finishTime)
                    );
            }
            else
            {
                GameObject hellriderRow = Instantiate(
                    hellriderPlacingRowPrefab,
                    rosterContainer.transform);

                hellriderRow.GetComponent<HellriderPlacingRow>().FillIn(
                    currentRoster[i].positionNr,
                    currentRoster[i].hellrider.hellriderData.hellriderIcon,
                    currentRoster[i].playerName,
                    ConvertToRaceTime(currentRoster[i].finishTime)
                    );
            }
        }
    }

    private string ConvertToRaceTime(float finishTime)
    {
        float minutes = Mathf.FloorToInt(finishTime / 60);
        float seconds = Mathf.FloorToInt(finishTime % 60);
        float milliSeconds = (finishTime % 1) * 1000;

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
    }
}
