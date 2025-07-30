using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyMemoryParameters : LevelParameters
{

    public int AnswersNumber { get; internal set; }
    public int NestsNumber { get; internal set; }
    public int NumberOfTargetedAreas { get; internal set; }
    public float ShowTime { get; internal set; }

    public void SetLevelParameters(int _AnswersNumber, int _NestsNumber, int _NumberOfTargetedAreas)
    {
        AnswersNumber = _AnswersNumber;
        NestsNumber = _NestsNumber;
        NumberOfTargetedAreas = _NumberOfTargetedAreas;
    }

    public void SetDifficultyParameters(float _ShowTime)
    {
        ShowTime = _ShowTime;
    }
}
