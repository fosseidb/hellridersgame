using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public string playerName;
    public int playerNr;

    public RacePositionData racePositiondata;

    public Player(string playerName,RacePositionData rpd)
    {
        this.playerName = playerName;
        this.racePositiondata = rpd;
    }

    public void SetName(string name)
    {
        this.playerName = name;
    }

    public void GiveRacePositionDataPoint(RacePositionData rpd)
    {
        racePositiondata = rpd;
    }

    public void SetPlayerID(int nr)
    {
        playerNr = nr;
    }
}
