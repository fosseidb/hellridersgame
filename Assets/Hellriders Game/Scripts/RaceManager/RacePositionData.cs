using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacePositionData : ScriptableObject
{
    public int positionNr;
    public string playerName;
    public Hellrider hellrider;
    public float finishTime;
    public int latestMilestone;

    public RacePositionData(int position, string playerName, Hellrider hellrider)
    {
        positionNr = position;
        this.playerName = playerName;
        this.hellrider = hellrider;
    }
}
