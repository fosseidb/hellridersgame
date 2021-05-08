using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIMainMenu : MonoBehaviour
{
    public enum MenuPage { Main, SelectCar, SeeBack}

    [SerializeField] private CinemachineMainMenuController _cmmc;

    [Header("Pages")]
    [SerializeField] private GameObject _mainMenuPage; // pageID = 0
    [SerializeField] private GameObject _settingsPage; // pageID = 1
    [SerializeField] private GameObject _creditsPage; // pageID = 2
    [SerializeField] private GameObject _selectLevelPage; // pageID = 3
    [SerializeField] private GameObject _SelectHellriderPage; // pageID = 4
    [SerializeField] private GameObject _SeeBackPage; // pageID = 5

    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown[] _HardPointDropdowns;

    [Header("Levels")]
    [SerializeField] private GameObject[] _levels;
    [SerializeField] private TMP_Text _levelTitle;
    [SerializeField] private TMP_Text _levelDescription;

    #region Leveltitles and descriptions
    private string[] _levelTitles;
    private string[] _levelDescriptions;
    private int _selectedLevel;
    #endregion

    private void Awake()
    {
        _levelTitles = new string[] { "Limbo", "Lust", "Gluttony", "Greed", "Anger", "Heresy", "Violence", "Fraud", "Treachery" };
        _levelDescriptions = new string[] { "Limbo is a place where one waits to be judged.",
            "Lust is a windy place where those who have carnally sinned never meet another.",
            "Gluttony, a landscape of wasteful corpulence.",
            "Greed, not all that glitters is gold, but all of it burdens.",
            "Anger leads to hate and hate leads to the dark side.",
            "Heresy, the sin of thinking differently. An open mind is like a fortress with its gates unbarred.",
            "Violence and death goes hand in hand",
            "Fraud deciet and lies is the venom that drips from split toungues.",
            "Treachery, to betray ones own is the darkest sin of all."
        };
        _selectedLevel = 0;
    }



    private void FixedUpdate()
    {
        for (int i = 0; i < _levels.Length; i++)
        {
            if (_selectedLevel == i)
            {
                _levels[i].GetComponent<Button>().Select();
            }
        }
    }


    public void SelectPage(int state)
    {
        //selects Cinemachine Camera.
        switch (state) 
        {
            case (0): // main menu
                _cmmc.OnClickMainMenu();
            break;

            case (4): // Select Hellrider
                _cmmc.OnClickHellrider();
                break;

            case (5): // View back
                _cmmc.OnClickHellriderBack();
                break;

            default:
                _cmmc.OnClickMainMenu();
                break;
        }
        SetActivePage(state);
    }

    private void SetActivePage(int pageID)
    {
        _mainMenuPage.SetActive(pageID == 0);
        _settingsPage.SetActive(pageID == 1);
        _creditsPage.SetActive(pageID == 2);
        _selectLevelPage.SetActive(pageID == 3);
        _SelectHellriderPage.SetActive(pageID == 4);
        _SeeBackPage.SetActive(pageID == 5);
    }

    public void UpdateHardPointDropdowns(int[] setup)
    {
        Debug.Log("In GUI, UPDATEHARDPOINTDDROPDOWN [" + setup[0] + setup[1] + setup[2] + setup[3] + "]");
        for (int i = 0; i < _HardPointDropdowns.Length; i++)
        {
            Debug.Log("int i: " + i + "setup i: " + setup[i+1]);
            _HardPointDropdowns[i].value = setup[i+1];
        }
    }

    public void UnlockLevels(int latestLevel)
    {
        Debug.Log("GUI Unlocking levels: " + latestLevel);
        for (int i = 0; i < _levels.Length; i++)
        {
            _levels[i].GetComponent<Button>().interactable = i <= latestLevel;
            Debug.Log("interactable: " + _levels[i].GetComponent<Button>().interactable);
            _levels[i].GetComponentsInChildren<Image>()[1].enabled = i > latestLevel;
            Debug.Log("chains enabled: " + _levels[i].GetComponentsInChildren<Image>()[1].enabled);
        }
    }

    public void SetHighlightedLevel(int level)
    {
        _selectedLevel = level;
        _levels[level].GetComponent<Button>().Select();
        _levelTitle.text = _levelTitles[level];
        _levelDescription.text = _levelDescriptions[level];
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
