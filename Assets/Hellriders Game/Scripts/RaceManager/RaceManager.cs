using System;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [Header("GUI & Camera")]
    public RaceGUIController _RGUIC;
    public CinemachineRaceController _CMRC;

    [Header("Hellrider")]
    public Hellrider _hellrider;
    public GameObject[] _hellriderLonglist;

    [Header("Race Map related")]
    public Transform _player1SpawnPoint;
    public FinishLine _finishLine;


    private StateMachine _stateMachine;

    //Stage 1 
    [Header("Stage 1")]
    public int _noLoadedPlayers;
    public int _noTotalPlayersInRace;

    //stage 2

    //stage 3
    private bool _finished;


    private void Awake()
    {
        _stateMachine = new StateMachine();
        _finishLine.OnCrossFinishLine += HandlePlayerCrossesFinishLine;

        //create stages
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
        Func<bool> AllPlayersLoaded() => () => _noLoadedPlayers == _noTotalPlayersInRace && loadIn._introCountdownTimer <= 0f;
        Func<bool> BeginRace() => () => countdown._raceCountdownTimer <= 0f;
        Func<bool> FinishRace() => () => _finished == true;

    }

    private void Update() => _stateMachine.Tick();

    // Start is called before the first frame update
    void Start()
    {

        _RGUIC = FindObjectOfType<RaceGUIController>();
        _hellrider = FindObjectOfType<Hellrider>();

        print("SelectedLevel: "+ PlayerPrefs.GetInt("selectedLevel"));
        print("SelectedCar: " + PlayerPrefs.GetInt("car"));
        print("SelectedHPF: " + PlayerPrefs.GetInt("frontHP"));
        print("SelectedHPT: " + PlayerPrefs.GetInt("topHP"));
        print("SelectedHPU: " + PlayerPrefs.GetInt("utilHP"));

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
        _finished = true;
    }
}
