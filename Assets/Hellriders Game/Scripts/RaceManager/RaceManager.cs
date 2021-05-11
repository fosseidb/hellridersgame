using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    [Header("GUI & Camera")]
    public RaceGUIController _raceGUIController;
    public CinemachineRaceController _cinemachineRaceController;
    public bool playIntro = true;
    public float introTimer = 10f;

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


    private void Start()
    {
        //find and connect the finish line to the Race Manager.
        _finishLine.OnCrossFinishLine += HandlePlayerCrossesFinishLine;
        
        //initialize the statemachine controlling the major race states.
        _stateMachine = new StateMachine();

        //check if want to play load in intro
        if (!playIntro)
            introTimer = 0f;

        //create stages for the state machine.
        var loadIn = new LoadInStage(this, introTimer);
        var countdown = new CountdownStage(this);
        var race = new RaceStage(this);
        var finish = new FinishStage(this);

        // connect our conditional transitions
        At(loadIn, countdown, AllPlayersLoaded());
        At(countdown, race, BeginRace());
        At(race, finish, FinishRace());

        //Connect "From any State" transitions


        void At(IState from, IState to, Func<bool> condition) =>
            _stateMachine.AddTransition(from, to, condition);

        //condition functions
        Func<bool> AllPlayersLoaded() => () => _noLoadedPlayers == _noTotalPlayersInRace && loadIn.introCountdownTimer <= 0f;
        Func<bool> BeginRace() => () => countdown._raceCountdownTimer <= 0f;
        Func<bool> FinishRace() => () => _finished == true;

        _raceGUIController = FindObjectOfType<RaceGUIController>();
        SpawnPlayer();

        
        //Set initial state
        _stateMachine.SetState(loadIn); 
    }

    private void Update() => _stateMachine.Tick();

    //// Start is called before the first frame update
    //void Start()
    //{

    //    //_hellrider = FindObjectOfType<Hellrider>();

    //    //print("SelectedLevel: "+ PlayerPrefs.GetInt("selectedLevel"));
    //    //print("SelectedCar: " + PlayerPrefs.GetInt("car"));
    //    //print("SelectedHPF: " + PlayerPrefs.GetInt("frontHP"));
    //    //print("SelectedHPT: " + PlayerPrefs.GetInt("topHP"));
    //    //print("SelectedHPU: " + PlayerPrefs.GetInt("utilHP"));

    //}

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

        //if local player, hook up GUI
        _raceGUIController.HookUpGUI(_hellrider);

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
