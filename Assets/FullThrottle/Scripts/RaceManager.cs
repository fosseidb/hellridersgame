using System;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{

    public RaceGUIController _RGUIC;

    public Hellrider _hellrider;

    public Transform _player1SpawnPoint;

    public GameObject[] _hellriderLonglist;


    private StateMachine _stateMachine;

    //Stage 1 
    [HideInInspector]
    public int _noLoadedPlayers;
    public int _noTotalPlayersInRace;

    //stage 2

    //stage 3


    private void Awake()
    {
        _stateMachine = new StateMachine();

        //create stages
        var loadIn = new LoadInStage(this);
        var countdown = new CountdownStage(this);
        var race = new RaceStage(this);

        // connect our conditional transitions
        At(loadIn, countdown, AllPlayersLoaded());
        At(countdown, race, BeginRace());

        //Connect "From any State" transitions

        //Set initial state
        _stateMachine.SetState(loadIn);

        void At(IState from, IState to, Func<bool> condition) =>
            _stateMachine.AddTransition(from, to, condition);

        //condition functions
        Func<bool> AllPlayersLoaded() => () => _noLoadedPlayers == _noTotalPlayersInRace;
        Func<bool> BeginRace() => () => countdown._raceCountdownTimer <= 0f;

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
}
