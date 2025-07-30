using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighwayParameters : LevelParameters
{

    public int entranceNumber { get; internal set; }
    public int lineNumber { get; internal set; }
    public int carRule { get; internal set; }
    public int entranceRule { get; internal set; }

    public float Speed { get; internal set; }
    public float TimeBetweenCarSpawns { get; internal set; }

    public void SetLevelParameters(int _intranceNumber, int _lineNumber, int _carRule, int _intranceRule)
    {
        entranceNumber = _intranceNumber;
        lineNumber = _lineNumber;
        carRule = _carRule;
        entranceRule = _intranceRule;
    }

    public void SetDifficultyParameters(float _Speed, float _TimeBetweenCarSpawns)
    {
        Speed = _Speed;
        TimeBetweenCarSpawns = _TimeBetweenCarSpawns;
    }
}
