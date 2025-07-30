using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainParameters : LevelParameters
{
    public int TrueWagonsNumber { get; internal set; }
    public int FalseWagonsNumber { get; internal set; }

    public float Speed { get; internal set; }

    public void SetLevelParameters(int _TrueWagonsNumber, int _FalseWagonsNumber)
    {
        TrueWagonsNumber = _TrueWagonsNumber;
        FalseWagonsNumber = _FalseWagonsNumber;
    }

    public void SetDifficultyParameters(float _Speed)
    {
        Speed = _Speed;
    }
}
