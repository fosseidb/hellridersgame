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
    [SerializeField] private LevelData[] levelData;
    [SerializeField] private TMP_Text _levelNameText;
    [SerializeField] private TMP_Text _leveldescriptionText;
    [SerializeField] private Image _levelImage;
    public GameObject levelButtonPrefab;
    public GameObject levelButtonContentHolder;
    private int _selectedLevel; 

    private void Awake()
    {
        foreach (LevelData level in levelData) {
            //instantiate level button and place in (set parent) the contentholder. Since the content holder has a grid greoup laoyt component i dont need to think about position.
            GameObject levelButtonObject = Instantiate(
                levelButtonPrefab, 
                levelButtonContentHolder.transform);

            LevelButton lvlBtn = levelButtonObject.GetComponent<LevelButton>();
            lvlBtn.FillButton(level);
            lvlBtn.OnThisLevelClicked += SetSelectedLevel;

        }

        //_levelTitles = new string[] { "Limbo", "Lust", "Gluttony", "Greed", "Anger", "Heresy", "Violence", "Fraud", "Treachery" };
        //_levelDescriptions = new string[] { "Limbo is a place where one waits to be judged.",
        //    "Lust is a windy place where those who have carnally sinned never meet another.",
        //    "Gluttony, a landscape of wasteful corpulence.",
        //    "Greed, not all that glitters is gold, but all of it burdens.",
        //    "Anger leads to hate and hate leads to the dark side.",
        //    "Heresy, the sin of thinking differently. An open mind is like a fortress with its gates unbarred.",
        //    "Violence and death goes hand in hand",
        //    "Fraud deciet and lies is the venom that drips from split toungues.",
        //    "Treachery, to betray ones own is the darkest sin of all."
        //};

        SetSelectedLevel(levelData[0]);
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

    public void SetSelectedLevel(LevelData lvlData)
    {
        Debug.Log("SetSelectedLevel triggered in GUI" + lvlData.levelID);
        _selectedLevel = lvlData.levelID;
        _levelNameText.text = lvlData.levelName;
        _leveldescriptionText.text = lvlData.levelDescription;
        _levelImage.sprite = lvlData.levelWallpaper;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
