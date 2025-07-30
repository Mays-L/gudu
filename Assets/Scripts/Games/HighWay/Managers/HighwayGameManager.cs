using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighwayGameManager: GameManager
{
    // Level factory
    HighwayLevelFactory myLevelFactory;

    protected override void Start()
    {
        myLevelFactory = LevelFactory.Instance.GetComponent<HighwayLevelFactory>();
        base.Start();
    }


    /// <summary>
    /// Adding Events and listeners for this game
    /// </summary>
    protected override void Add_Events_Listeners()
    {
        base.Add_Events_Listeners();
        #region Adding The Events
        // Addig a event with a GameObject parameter. 
        // Every time the player touch (or click) a collider this event will be invoked.
        EventManager.Instance.AddGameObjectEvent("TouchedGameObject");
        #endregion

    }

    #region Level and finished handling
    public override void ShowLevel()
    {
        if (!ShowLocked)
        {
            base.ShowLevel();
            GetParameters();
            Timers.Instance.StartTimer("LevelTimer", 0f);
            CarManager.Instance.hiddenDataEncoder.difficulty = LevelFactory.Instance.CurrentDifficulty;
            Debug.Log(LevelFactory.Instance.CurrentDifficulty);
            CarManager.Instance.carsSpeed = myLevelFactory.parameters.Speed;
            CarManager.Instance.TimeBetweenCarSpawns = myLevelFactory.parameters.TimeBetweenCarSpawns;
            CarManager.Instance.StartSpawning(myLevelFactory.parameters);
            Timers.Instance.StartTimer("ResponseTimer", 0);
            Timers.Instance.StartAnswerTimer();
            ShowLocked = true;
        }
    }

    public override void HideLevel()
    {
        if (!IsHideLevel)
        {
            ShowLocked = false;
            base.HideLevel();
            CarManager.Instance.StopSpawning();
            Timers.Instance.StartTimer(1.5f, ProcessGameStates);
        }
    }
    #endregion

    #region Processing Results

    protected override void AddToTrueAnswers()
    {
        if (FinishedTutorial() && !Tutorial.activeSelf)
            LevelFactory.Instance.LevelDifficultyController(true);
        base.AddToTrueAnswers();
    }

    protected override void AddToFalseAnswers()
    {
        if (FinishedTutorial() && !Tutorial.activeSelf)
            LevelFactory.Instance.LevelDifficultyController(false);
        base.AddToFalseAnswers();
    }

    protected override void CheckEndOfLevel()
    {
        if (FinishedTutorial() && !Tutorial.activeSelf)
        {
            if (LevelFactory.Instance.CurrentState == LevelDiffStates.LevelChanged)
            {
                Get_Send_GameData(false);
                ResultsHandling.Instance.ResetWrongAndRightSelectionCounters();
                HideLevel();
            }
            LevelFactory.Instance.UpdateLevelDifficulty();
            CarManager.Instance.hiddenDataEncoder.difficulty = LevelFactory.Instance.CurrentDifficulty;
            CarManager.Instance.carsSpeed = myLevelFactory.parameters.Speed;
            CarManager.Instance.TimeBetweenCarSpawns = myLevelFactory.parameters.TimeBetweenCarSpawns;
        }
        Timers.Instance.StartTimer("ResponseTimer", 0);
        Timers.Instance.StartAnswerTimer();
    }

    public override string GetSequentialHiddenData()
    {
        return CarManager.Instance.hiddenDataEncoder.EncodingSequentialData();
    }

    public override string GetCommonHiddenData()
    {
        return CarManager.Instance.hiddenDataEncoder.EncodingCommonData();
    }

    #endregion

    #region Calculating Parameters
    public override void GetParameters()
    {
        LevelFactory.Instance.GetParameters();
    }
    #endregion

    #region Tutorial Handling
    internal override Vector3 GetAnswerPosition()
    {
        return CarManager.Instance.GetTheAnswerPosition();
    }

    internal override bool FinishedTutorial()
    {
        if (ResultsHandling.Instance.GameTrueAnswerCounter >= 3 || !isInTutorial || RunType == 2)
        {
            return true;
        }
        return false;
    }
    #endregion
}
