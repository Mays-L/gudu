using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represent Game Result parameters
/// </summary>
public class GameResults
{

    /// <summary>
    /// Task identifier
    /// </summary>
    public long UserAssignmentId;

    /// <summary>
    /// Game level
    /// </summary>
    public int Level;

    /// <summary>
    /// Number of true answers
    /// </summary>
    public int TrueAnsNum;

    /// <summary>
    /// Number of false answers
    /// </summary>
    public int FalseAnsNum;

    /// <summary>
    /// Detail parameters like rue false sequence
    /// </summary>
    public string Details;

    /// <summary>
    /// Game score
    /// </summary>
    public int Score;

    /// <summary>
    /// Is game finished or not 
    /// </summary>
    public bool IsDone;


    /// <summary>
    /// Game Duration
    /// </summary>
    public int DurationTime;


    /// <summary>
    /// True false answer sequence
    /// </summary>
    public string TrueFalseSeq;

    /// <summary>
    /// Answer time sequence
    /// </summary>
    public string ReactionTimes;

    /// <summary>
    /// Screen height
    /// </summary>
    public int ScreenHeight;

    /// <summary>
    /// Screen width
    /// </summary>
    public int ScreenWidth;

    public GameResults(int trueAnswerNum, int falseAnswerNum, int score, bool isDone, int deuration, string details, int level, int repeat, string trueFalseSeq, string ansTimeSequnce, int swidth, int sheight)
    {
        TrueAnsNum = trueAnswerNum;
        FalseAnsNum = falseAnswerNum;
        Score = score;
        IsDone = isDone;
        DurationTime = deuration;
        Details = details;
        Level = level;
        //StartTime = DateTime.Now.ToString();
        //Repeat = repeat;
        TrueFalseSeq = trueFalseSeq;
        ReactionTimes = ansTimeSequnce;
        ScreenHeight = sheight;
        ScreenWidth = swidth;
    }

}