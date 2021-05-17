using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GUIMainMenu _gUIMainMenu;

    [SerializeField] private GameObject[] _hellriders;
    private Modification[] _FrontHardMods;
    private Modification[] _TopHardMods;
    private Modification[] _UtilHardMods;

    private int[] _selectedSetup; // 0-Car, 1-frontHP, 2-topHP, 3-utilHP
    private int _latestUnlockedLevel;
    private int _selectedLevel;

    private void Start()
    {
        //GUI Page Set up correct page
        _gUIMainMenu.SelectPage(0);

        // Hellrider Setup - if the player already has a preferred car selceted select this, else go for default setup
        if(PlayerPrefs.GetInt("hasSaved") == 0)
            _selectedSetup = new int[] { 0, 0, 0, 0 };
        else
            _selectedSetup = new int[] { PlayerPrefs.GetInt("car"), PlayerPrefs.GetInt("frontHP"), PlayerPrefs.GetInt("topHP"), PlayerPrefs.GetInt("utilHP") };
        OnSelectCarClick(_selectedSetup[0]);
        Debug.Log("Playerprefs: " + PlayerPrefs.GetInt("car") + PlayerPrefs.GetInt("frontHP") + PlayerPrefs.GetInt("topHP") + PlayerPrefs.GetInt("utilHP"));

        // Level Setup - check what the latest level the player has played and unlock these and then select level 0
        //_latestUnlockedLevel = PlayerPrefs.HasKey("latestLevel") ? PlayerPrefs.GetInt("latestLevel") : 4;
        //UnlockLevels();
        //SelectLevel(0);
    }

    /// <summary>
    /// Unlocks the levels that are stored as unlocked in player prefs.
    /// </summary>
    //private void UnlockLevels()
    //{
    //    Debug.Log("MMM Unlocking levels: " + _latestUnlockedLevel);
    //    _gUIMainMenu.UnlockLevels(_latestUnlockedLevel);
    //}

    /// <summary>
    /// Selects the level to be highlighted.
    /// </summary>
    /// <param name="i"> integer for level to be selected </param>
    //public void SelectLevel(int i)
    //{
    //    _selectedLevel = i;
    //    _gUIMainMenu.SetHighlightedLevel(i);
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="activeCarIndex">The index of the active car</param>
    public void OnSelectCarClick(int activeCarIndex)
    {
        _selectedSetup[0] = activeCarIndex;
        for (int i = 0; i < _hellriders.Length; i++)
        {
            _hellriders[i].SetActive(i == activeCarIndex);
        }

        GameObject[] _loadedHardpoints = _hellriders[activeCarIndex].GetComponent<Hellrider>().PresentHardpoints();
        
        _FrontHardMods = _loadedHardpoints[0].GetComponentsInChildren<Modification>(true);
        _TopHardMods = _loadedHardpoints[1].GetComponentsInChildren<Modification>(true); ;
        _UtilHardMods = _loadedHardpoints[2].GetComponentsInChildren<Modification>(true); ;

        _gUIMainMenu.UpdateHardPointDropdowns(_selectedSetup);
    }

    public void OnSelectTopHardpointClick(int activeHardpoint)
    {
        _selectedSetup[2] = activeHardpoint;
        _hellriders[_selectedSetup[0]].GetComponent<Hellrider>().UpdateHardpoints(_selectedSetup);

    }

    public void OnSelectFrontHardpointClick(int activeHardpoint)
    {
        _selectedSetup[1] = activeHardpoint;
        _hellriders[_selectedSetup[0]].GetComponent<Hellrider>().UpdateHardpoints(_selectedSetup);
    }

    public void OnSelectUtilHardpointClick(int activeHardpoint)
    {
        _selectedSetup[3] = activeHardpoint;
        _hellriders[_selectedSetup[0]].GetComponent<Hellrider>().UpdateHardpoints(_selectedSetup);
    }

    public void StartRace()
    {
        //set player selection and savedState
        PlayerPrefs.SetInt("car", _selectedSetup[0]);
        PlayerPrefs.SetInt("frontHP", _selectedSetup[1]);
        PlayerPrefs.SetInt("topHP", _selectedSetup[2]);
        PlayerPrefs.SetInt("utilHP", _selectedSetup[3]);
        PlayerPrefs.SetInt("hasSaved", 1);

        //select the right level
        PlayerPrefs.SetInt("selectedLevel", _selectedLevel);
        SceneManager.LoadScene(1); //loads level 1
    }

}
