using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HellriderPlacingRow : MonoBehaviour
{
    public TMP_Text finalposition;
    public Image driverIcon;
    public TMP_Text racerNameText;
    public TMP_Text finalTimeText;

    public void FillIn(int pos, Sprite dIcon, string racerName, string finalTime)
    {
        finalposition.text = pos.ToString();
        driverIcon.sprite = dIcon;
        racerNameText.text = racerName;
        finalTimeText.text = finalTime;
    }
}
