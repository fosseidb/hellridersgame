using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{

    public LevelData levelData;

    public Image backgroundImage;
    public TMP_Text levelText;
    public Image chains;

    public delegate void MyButtonClickedEvent(LevelData lvlData);
    public event MyButtonClickedEvent OnThisLevelClicked;

    public void FillButton(LevelData levelData)
    {
        this.levelData = levelData;

        backgroundImage.sprite = levelData.levelWallpaper;
        levelText.text = levelData.levelName;
        GetComponent<Button>().interactable = levelData.unlocked;
        chains.enabled = !levelData.unlocked;
    }

    public void ButtonClicked()
    {
        Debug.Log("OnMouseUp cpressed");
        OnThisLevelClicked?.Invoke(levelData);
    }

    
}
