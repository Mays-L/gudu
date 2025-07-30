using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HoneyMemoryGameManager : GameManager
{

    // Level factory
    HoneyMemoryLevelFactory myLevelFactory;

    public GameObject FlyingBee;

    protected override void Start()
    {
        myLevelFactory = LevelFactory.Instance.GetComponent<HoneyMemoryLevelFactory>();
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

        // When the distinction starts, this event will be invoked. It's useful for nests to change their sprites to  Distinct sprite.
        EventManager.Instance.AddNoParameterEvent("StartingDistinctionEvent");
        // When the distinction ends, this event will be invoked. It's useful for nests to change their sprites back to its default sprite.
        EventManager.Instance.AddNoParameterEvent("EndingDistinctionEvent");
        #endregion
    }



    #region Level and finished handling
    public override void ShowLevel()
    {
        if (!ShowLocked)
        {
            base.ShowLevel();
            GetParameters();
            NestsManager.Instance.ShowNests(myLevelFactory.parameters.NestsNumber, myLevelFactory.parameters.AnswersNumber, myLevelFactory.parameters.NumberOfTargetedAreas);

            float DistractorNum = UnityEngine.Random.Range(0f, 1f);
            if (DistractorNum <= (float)LevelFactory.Instance.RequiredLevels[0].Distractor / 100)
            {
                FlyBee();
                NestsManager.Instance.hiddenDataEncoder.distractor = 1;
            }
            else
                NestsManager.Instance.hiddenDataEncoder.distractor = 0;
            NestsManager.Instance.hiddenDataEncoder.difficulty = LevelFactory.Instance.CurrentDifficulty;

            Timers.Instance.StartTimer("LevelTimer", myLevelFactory.parameters.ShowTime);
            NestsManager.Instance.ShowDistinct(myLevelFactory.parameters.ShowTime);
            //Timers.Instance.StartTimer("ResponseTimer", myLevelFactory.parameters.ShowTime);
            //Timers.Instance.StartAnswerTimer(myLevelFactory.parameters.ShowTime);
            ShowLocked = true;
        }
    }

    public override void HideLevel()
    {
        if (!IsHideLevel)
        {
            ShowLocked = false;
            base.HideLevel();
            NestsManager.Instance.RemoveAllNests();
            ResultsHandling.Instance.ResetWrongAndRightSelectionCounters();
            Timers.Instance.StartTimer(1.5f, ProcessGameStates);

            FlyingBee.SetActive(false);
        }
    }
    #endregion

    #region Processing Results

    protected override void CheckEndOfLevel()
    {
        int AllAnswers = ResultsHandling.Instance.AllAnswers;
        int trueAnswers = ResultsHandling.Instance.LevelTrueAnswersCounter;
        int wrongAnswers = ResultsHandling.Instance.LevelFalseAnswersCounter;
        if (AllAnswers >= myLevelFactory.parameters.AnswersNumber)
        {
            if (FinishedTutorial() && !Tutorial.activeSelf)
            {
                Get_Send_GameData(false);
                LevelFactory.Instance.LevelDifficultyController(trueAnswers == myLevelFactory.parameters.AnswersNumber);
                LevelFactory.Instance.UpdateLevelDifficulty();
            }

            NestsManager.Instance.DisableSelecting();
            Timers.Instance.StartTimer(1f, HideLevel);
        }
        Timers.Instance.StartTimer("ResponseTimer", 0f);
        Timers.Instance.StartAnswerTimer(0f);
    }

    protected override void UpdateResultsOnScoreBoard()
    {
        base.UpdateResultsOnScoreBoard();
        scoreBoard.SetProgress(ResultsHandling.Instance.LevelTrueAnswersCounter, myLevelFactory.parameters.AnswersNumber);
    }

    public override string GetSequentialHiddenData()
    {
        return NestsManager.Instance.hiddenDataEncoder.EncodingSequentialData();
    }

    public override string GetCommonHiddenData()
    {
        return NestsManager.Instance.hiddenDataEncoder.EncodingCommonData();
    }
    #endregion

    #region Calculating Parameters
    public override void GetParameters()
    {
        LevelFactory.Instance.GetParameters();
    }
    #endregion

    private void FlyBee()
    {
        FlyingBee.SetActive(true);
        FlyingBee.GetComponent<Animator>().Play("BeeFlying");
    }

    #region Tutorial Handling
    internal override Vector3 GetAnswerPosition()
    {
        return NestsManager.Instance.GetTheAnswerPosition();
    }

    internal override bool FinishedTutorial()
    {
        if (ResultsHandling.Instance.AllAnswers >= myLevelFactory.parameters.AnswersNumber || !isInTutorial || RunType == 2)
        {
            return true;
        }
        return false;
    }


    #endregion
}
