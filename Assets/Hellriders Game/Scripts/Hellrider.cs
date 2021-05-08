using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellrider : MonoBehaviour
{
    [SerializeField] private GameObject _frontHardpoint;
    [SerializeField] private GameObject _topHardpoint;
    [SerializeField] private GameObject _utilHardpoint;

    private Modification[] _frontHardMods;
    private Modification[] _topHardMods;
    private Modification[] _UtilHardMods;

    private GameObject[] _hardPoints;

    public GameObject TopHardpoint { get => _topHardpoint; set => _topHardpoint = value; }

    public void Awake()
    {
        _hardPoints = new GameObject[] { _frontHardpoint, _topHardpoint, _utilHardpoint };

        _frontHardMods = _frontHardpoint.GetComponentsInChildren<Modification>(true);
        _topHardMods = _topHardpoint.GetComponentsInChildren<Modification>(true);
        _UtilHardMods = _utilHardpoint.GetComponentsInChildren<Modification>(true);

        int[] loadedSetup = new int[] {
            0,
            PlayerPrefs.GetInt("frontHP"),
            PlayerPrefs.GetInt("topHP"),
            PlayerPrefs.GetInt("utilHP"),
        };

        UpdateHardpoints(loadedSetup);        
    }
    public GameObject[] PresentHardpoints()
    {
        return _hardPoints;
    }


    public void UpdateHardpoints(int[] setup)
    {
        Debug.Log("In Hellrider, UPDATEHARDPOINTS [" + setup[0] + setup[1] + setup[2] + setup[3] + "]");
        UpdateHardPoint(_frontHardMods, setup[1]);
        UpdateHardPoint(_topHardMods, setup[2]);
        UpdateHardPoint(_UtilHardMods, setup[3]);
    }

    private void UpdateHardPoint(Modification[] hardpoint, int activeHardpoint)
    {
        for (int i = 0; i < _topHardMods.Length; i++)
        {
            hardpoint[i].gameObject.SetActive(i == activeHardpoint);
        }
    }
}
