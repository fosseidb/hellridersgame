using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RaceManager : MonoBehaviour
{
    [Header("GUI & Camera")]
    public RaceGUIController _raceGUIController;
    public CinemachineRaceController _cinemachineRaceController;
    public bool playIntro = true;
    public float introTimer = 10f;

    [Header("Hellriders")]
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
    public Player _localHellriderPlayer;

    //state 2 - countdown

    //stage 3
    [Header("Stage 3 - Race")]
    public float raceTimer;
    //public Dictionary<int, Player> players;
    public List<Player> players;

    //stage 4
    [Header("Stage 4 - Finish")]
    public List<RacePositionData> racePositions;
    private bool _raceFinished;
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
        Func<bool> FinishRace() => () => _raceFinished == true;

        //connect GUI system
        _raceGUIController = FindObjectOfType<RaceGUIController>();

        //spawn players
        players = new List<Player>();
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
        Hellrider _hellrider = go.GetComponent<Hellrider>();

        //Add all components to vehicle
        VehicleUserController vuc = go.AddComponent(typeof(VehicleUserController)) as VehicleUserController;
        GiveHellridersUserControlAccess(_hellrider, false);
        WeaponsAndUtilModsController tc = go.AddComponent(typeof(WeaponsAndUtilModsController)) as WeaponsAndUtilModsController;
        FallOffTrackRespawner fotr = go.AddComponent(typeof(FallOffTrackRespawner)) as FallOffTrackRespawner;
        
        //player
        Player player = go.AddComponent(typeof(Player)) as Player;
        player.SetName("John");
        player.SetPlayerID(_noLoadedPlayers+1);
        players.Add(player);
        _localHellriderPlayer = player;

        //place vehicle on spawnpoint
        fotr._initialRespawnPoint = _player1SpawnPoint;

        // create positionDataPoint for player
        RacePositionData racePositionData = new RacePositionData(player.playerNr, player.name, _hellrider);
        racePositions.Add(racePositionData);
        player.GiveRacePositionDataPoint(racePositionData);
        go.GetComponent<FallOffTrackRespawner>().OnMilestonePassed += HellriderPassedMilestone;


        //if local player, hook up GUI
        _raceGUIController.HookUpGUI(_hellrider);

        //update number of loaded players
        _noLoadedPlayers++;
    }

    private void HellriderPassedMilestone(Player player, int milestoneID)
    {
        player.racePositiondata.latestMilestone = milestoneID;

        players.Sort((o1, o2) => o1.racePositiondata.latestMilestone.CompareTo(o2.racePositiondata.latestMilestone));
        _raceGUIController.UpdateFinishRoster(racePositions);
    }

    private void HandlePlayerCrossesFinishLine(Player player)
    {
        Debug.Log("Player crosses finish line!");

        //register rider
        player.racePositiondata.finishTime = raceTimer;   
        
        _raceGUIController.UpdateFinishRoster(racePositions);

        //trigger next stage
        _raceFinished = true;
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
