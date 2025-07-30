using Assets.Scripts.Behaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsGameManager : GameManager
{
    [SerializeField]
    GameObject HelpDragDrop;

    // Level factory
    CloudsLevelFactory myLevelFactory;

    protected override void Start()
    {
        myLevelFactory = LevelFactory.Instance.GetComponent<CloudsLevelFactory>();
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
        EventManager.Instance.AddNoParameterEvent("StartingMovingEvent");
        // When the distinction ends, this event will be invoked. It's useful for nests to change their sprites back to its default sprite.
        EventManager.Instance.AddNoParameterEvent("EndingMovingEvent");
        #endregion
        EventManager.Instance.AddListeners("StartingMovingEvent", AudioManager.Instance.PlayMainClip);
        EventManager.Instance.AddListeners("EndingMovingEvent", StopCLoudsShowHelp);
    }


    #region Level and finished handling
    public override void ShowLevel()
    {
        if (!ShowLocked)
        {
            base.ShowLevel();

            GetParameters();
            CloudsManager.Instance.ShowClouds(myLevelFactory.parameters.CloudsNumber, myLevelFactory.parameters.RainyCloudsNumber, myLevelFactory.parameters.NumberOfRainyAreas, myLevelFactory.parameters.InitialSpeed, myLevelFactory.parameters.Acceleration);
            CloudsManager.Instance.hiddenDataEncoder.difficulty = LevelFactory.Instance.CurrentDifficulty;

            float DistractorNum = UnityEngine.Random.Range(0f, 1f);
            int _distInt;
            if (DistractorNum <= (float)LevelFactory.Instance.RequiredLevels[0].Distractor / 100)
                _distInt = UnityEngine.Random.Range(1, 4);
            else
                _distInt = 0;
            DestructorsManager.Instance.SetDestructor((DistractorType)_distInt);
            CloudsManager.Instance.hiddenDataEncoder.distractor = _distInt;

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

            CloudsManager.Instance.RemoveAllClouds();
            ResultsHandling.Instance.ResetWrongAndRightSelectionCounters();
            Timers.Instance.StartTimer(1f, ProcessGameStates);
            DestructorsManager.Instance.SetAllDeactive();
        }
    }
    #endregion

    #region Processing Results

    protected override void CheckEndOfLevel()
    {
        int AllAnswers = ResultsHandling.Instance.AllAnswers;
        int trueAnswers = ResultsHandling.Instance.LevelTrueAnswersCounter;
        int wrongAnswers = ResultsHandling.Instance.LevelFalseAnswersCounter;
        if (AllAnswers >= myLevelFactory.parameters.RainyCloudsNumber)
        {
            if (FinishedTutorial() && !Tutorial.activeSelf)
            {
                Get_Send_GameData(false);
                LevelFactory.Instance.LevelDifficultyController(trueAnswers == myLevelFactory.parameters.RainyCloudsNumber);
                LevelFactory.Instance.UpdateLevelDifficulty();
            }

            CloudsManager.Instance.DisableTouching();
            Timers.Instance.StartTimer(1f, HideLevel);
        }

        Timers.Instance.StartTimer("ResponseTimer", 0);
        Timers.Instance.StartAnswerTimer();
    }

    protected override void UpdateResultsOnScoreBoard()
    {
        base.UpdateResultsOnScoreBoard();
        scoreBoard.SetProgress(ResultsHandling.Instance.LevelTrueAnswersCounter, myLevelFactory.parameters.RainyCloudsNumber);
    }

    public override string GetSequentialHiddenData()
    {
        return CloudsManager.Instance.hiddenDataEncoder.EncodingSequentialData();
    }

    public override string GetCommonHiddenData()
    {
        return CloudsManager.Instance.hiddenDataEncoder.EncodingCommonData();
    }
    #endregion

    #region Calculating Parameters
    public override void GetParameters()
    {
        LevelFactory.Instance.GetParameters();
    }
    #endregion

    #region UI 
    private void StopCLoudsShowHelp()
    {
        if (CurrentState == GameState.RUNNING)
        {
            //AudioManager.Instance.PlayStopClip();
        }
    }

    private void ShowDragDropHelp()
    {
        if (FirstEnter)
        {
            TutorialDialog = true;
            DisableUIElements();
            HelpDragDrop.SetActive(true);
            FirstEnter = false;
        }
        else
        {
            HelpDragDrop.SetActive(false);
        }
    }
    public void ResetTime()
    {
        ResetLevel();
        repeat--;
        scoreBoard.SetTime(GameManager.Instance.GameTime);
    }

    public override void ReloadGame()
    {
        TutorialDialog = false;
        base.ReloadGame();
        HelpDragDrop.SetActive(false);
    }
    #endregion

    #region Tutorial Handling
    public  void DisableDropHelpDialog()
    {
        TutorialDialog = false;
        HideLevel();
        HelpDragDrop.SetActive(false);
    }
    internal override Vector3 GetAnswerPosition()
    {
        return CloudsManager.Instance.GetTheAnswerPosition();
    }
    internal override void EndOfTutorial()
    {
        ShowDragDropHelp();
        base.EndOfTutorial();
    }
    internal override bool FinishedTutorial()
    {
        if (ResultsHandling.Instance.AllAnswers >= myLevelFactory.parameters.RainyCloudsNumber || !isInTutorial || RunType == 2)
        {
            return true;
        }
        return false;
    }
    #endregion
}
