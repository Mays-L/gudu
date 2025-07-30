using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsHandling : Singleton<ResultsHandling>
{
    public int Score { get; private set; }
    public int LevelFalseAnswersCounter { get; private set; } = 0;
    public int LevelTrueAnswersCounter { get; private set; } = 0;
    public int GameTrueAnswerCounter { get; private set; } = 0;
    public int GameFalseAnswerCounter { get; private set; } = 0;
    public int AllAnswers { get; private set; } = 0;

    public string TrueFalseSequence="";

    public string ResponseTimeSequence = "";

    public string AnswerTimeSequence = "";

    public string HiddenDataSequence = "";

    void Start()
    {
        ResetScore();
    }

    ///<summary>
    /// Adding to the score, level, right and wrong selections
    /// </summary>
    public void AddToScore(int amount) 
    { 
        Score += amount;
    }
    public void AddToTrueSelections()
    {
        GameTrueAnswerCounter++;
        LevelTrueAnswersCounter++;
        AllAnswers++;
        TrueFalseSequence += "T";

        if (!HiddenDataSequence.Equals(""))
            HiddenDataSequence += ",";
        HiddenDataSequence += GameManager.Instance.GetSequentialHiddenData();
    }
    public void AddToFalseSelections()
    {
        GameFalseAnswerCounter++;
        LevelFalseAnswersCounter++;
        AllAnswers++;
        TrueFalseSequence += "F";

        if (!HiddenDataSequence.Equals(""))
            HiddenDataSequence += ",";
        HiddenDataSequence += GameManager.Instance.GetSequentialHiddenData();
    }
    /// <summary>
    ///  Subtract from the score
    /// </summary>
    public void SubtractFromScore(int amount)
    {
        Score -= Score - amount >= 0 ? amount : Score;
    }

    /// <summary>
    /// Reset the score and counters
    /// </summary>
    public void ResetScore()
    {
        Score = 0;
        GameFalseAnswerCounter = 0;
        GameTrueAnswerCounter = 0;
        ResetWrongAndRightSelectionCounters();
        
    }
    public void ResetWrongAndRightSelectionCounters() 
    {
        LevelTrueAnswersCounter = 0;
        LevelFalseAnswersCounter = 0;
        TrueFalseSequence = "";
        ResponseTimeSequence = "";
        AnswerTimeSequence = "";
        HiddenDataSequence = "";
        AllAnswers = 0;
    }

    /// <summary>
    /// Get user progress
    /// </summary>
    /// <returns>Float between 0 to 1</returns>
    public float GetProgress()
    {
        return 0.4f;
    }

    /// <summary>
    /// Get user performance to other users
    /// </summary>
    /// <returns>Float between 0 to 1</returns>
    public float GetPerformance()
    {
        return 0.6f  ;
    }

    /// <summary>
    /// Get percent of true answers in the whole game
    /// </summary>
    /// <returns>Percent between 0 to 1</returns>
    public float GetTruePercent()
    {
        if (AllAnswers > 0)
            return (float)GameTrueAnswerCounter / (float)(GameTrueAnswerCounter + GameFalseAnswerCounter);
        else
            return 0;
    }

    internal void AddToResponseTimes(int time, int time2)
    {
        if (!ResponseTimeSequence.Equals(""))
            ResponseTimeSequence += ",";
        ResponseTimeSequence += time;

        if (!AnswerTimeSequence.Equals(""))
            AnswerTimeSequence += ",";
        AnswerTimeSequence += time2;
    }

    internal string GetDetails() 
    {
        string Details = "TFS:[" + TrueFalseSequence + "] RTS:[" + ResponseTimeSequence + "] HDS:[" + HiddenDataSequence + "] CHD:[" + GameManager.Instance.GetCommonHiddenData() + "]"; 
        return Details;
    }
}
