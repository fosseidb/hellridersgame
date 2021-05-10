using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    [Header("GUI & Camera")]
    public RaceGUIController _RGUIC;
    public CinemachineRaceController _CMRC;

    [Header("Hellrider")]
    public Hellrider _hellrider;
    public GameObject[] _hellriderLonglist;

    [Header("Race Map related")]
    public int raceLevelID;
    public string raceLevelName;
    public Transform _player1SpawnPoint;
    public FinishLine _finishLine;


    private StateMachine _stateMachine;

    //Stage 1 
    [Header("Stage 1 - load in")]
    public int _noLoadedPlayers;
    public int _noTotalPlayersInRace;

    //state 2 - countdown

    //stage 3
    [Header("Stage 3 - Race")]
    public float raceTimer;

    //stage 4
    [Header("Stage 4 - Finish")]
    public Dictionary<Hellrider, float> finishRoster = new Dictionary<Hellrider, float>();
    private bool _finished;
    private float _finishTime;


    private void Awake()
    {
        //find and connect the finish line to the Race Manager.
        _finishLine.OnCrossFinishLine += HandlePlayerCrossesFinishLine;
        
        //initialize the statemachine controlling the major race states.
        _stateMachine = new StateMachine();

        //create stages for the state machine.
        var loadIn = new LoadInStage(this);
        var countdown = new CountdownStage(this);
        var race = new RaceStage(this);
        var finish = new FinishStage(this);

        // connect our conditional transitions
        At(loadIn, countdown, AllPlayersLoaded());
        At(countdown, race, BeginRace());
        At(race, finish, FinishRace());

        //Connect "From any State" transitions

        //Set initial state
        _stateMachine.SetState(loadIn);

        void At(IState from, IState to, Func<bool> condition) =>
            _stateMachine.AddTransition(from, to, condition);

        //condition functions
        Func<bool> AllPlayersLoaded() => () => _noLoadedPlayers == _noTotalPlayersInRace && loadIn.introCountdownTimer <= 0f;
        Func<bool> BeginRace() => () => countdown._raceCountdownTimer <= 0f;
        Func<bool> FinishRace() => () => _finished == true;

    }

    private void Update() => _stateMachine.Tick();

    // Start is called before the first frame update
    void Start()
    {

        _RGUIC = FindObjectOfType<RaceGUIController>();
        //_hellrider = FindObjectOfType<Hellrider>();

        //print("SelectedLevel: "+ PlayerPrefs.GetInt("selectedLevel"));
        //print("SelectedCar: " + PlayerPrefs.GetInt("car"));
        //print("SelectedHPF: " + PlayerPrefs.GetInt("frontHP"));
        //print("SelectedHPT: " + PlayerPrefs.GetInt("topHP"));
        //print("SelectedHPU: " + PlayerPrefs.GetInt("utilHP"));

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        print("Spawning player: " + PlayerPrefs.GetInt("car"));

        //instantiate and get components
        GameObject go = Instantiate(_hellriderLonglist[PlayerPrefs.GetInt("car")], _player1SpawnPoint.position, _player1SpawnPoint.rotation);
        _hellrider = go.GetComponent<Hellrider>();

        //Add all components to vehicle
        VehicleUserController vuc = _hellrider.gameObject.AddComponent(typeof(VehicleUserController)) as VehicleUserController;
        GiveHellridersUserControlAccess(_hellrider, false);
        TurretController tc = _hellrider.gameObject.AddComponent(typeof(TurretController)) as TurretController;
        FallOffTrackRespawner fotr = _hellrider.gameObject.AddComponent(typeof(FallOffTrackRespawner)) as FallOffTrackRespawner;
        
        //place vehicle on spawnpoint
        fotr._initialRespawnPoint = _player1SpawnPoint;

        //update number of loaded players
        _noLoadedPlayers++;
    }

    private void HandlePlayerCrossesFinishLine(GameObject hellrider)
    {
        Debug.Log("Rider crosses finish line!");

        //register rider
        Hellrider finishedHellrider = hellrider.GetComponent<Hellrider>();
        _finishTime = raceTimer;
        finishRoster.Add(finishedHellrider, _finishTime);

        //trigger next stage
        _finished = true;
    }

    public void CloseRace()
    {
        //If we wont his race
        CheckIfTimeGoodEnoughToCompleteRace();

        //if this is a new achievment, store this race as latest completed race to unlock next.
        if(PlayerPrefs.GetInt("latestLevel") < raceLevelID)
            PlayerPrefs.SetInt("latestLevel", raceLevelID);

        //load main menu
        SceneManager.LoadScene(0);

    }

    private void CheckIfTimeGoodEnoughToCompleteRace()
    {
        //checking
        Debug.Log("Checking if good enough");
    }

    public void SetMouseLock(bool toBeLocked)
    {
        if (toBeLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void GiveHellridersUserControlAccess(Hellrider hellrider, bool access)
    {
        hellrider.GetComponent<VehicleUserController>().GiveHellriderUserControllAccess(access);
    }
}
