using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGameManager : GameManager
{
    // Level factory
    TrainLevelFactory myLevelFactory;

    public Animator TopWasButton;

    protected override void Start()
    {
        myLevelFactory = LevelFactory.Instance.GetComponent<TrainLevelFactory>();
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
        EventManager.Instance.AddNoParameterEvent("NewWagonEnter");
        #endregion
    }

    #region Level handling

    public override void ShowLevel()
    {
        if (!ShowLocked)
        {
            base.ShowLevel();
            GetParameters();
            WagonsManager.Instance.ShowWagons(myLevelFactory.parameters.Speed, myLevelFactory.parameters.TrueWagonsNumber, myLevelFactory.parameters.FalseWagonsNumber);
            ShowLocked = true;
        }
    }

    public override void HideLevel()
    {
        if (!IsHideLevel)
        {
            ShowLocked = false;
            base.HideLevel();
            WagonsManager.Instance.RemoveAllWagons();
            ResultsHandling.Instance.ResetWrongAndRightSelectionCounters();
            Timers.Instance.StartTimer(2.5f, ProcessGameStates);
        }
    }

    #endregion

    #region Processing Results

    protected override void AddToTrueAnswers()
    {
        TopWasButton.SetTrigger("TrueResponse");
        if (FinishedTutorial() && !Tutorial.activeSelf)
            LevelFactory.Instance.LevelDifficultyController(true);
        base.AddToTrueAnswers();
    }

    protected override void AddToFalseAnswers()
    {
        TopWasButton.SetTrigger("FalseResponse");
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
                HideLevel();
            }
            LevelFactory.Instance.UpdateLevelDifficulty();
            WagonsManager.Instance.hiddenDataEncoder.difficulty = LevelFactory.Instance.CurrentDifficulty;
            WagonsManager.Instance.SetTrainSpeed(myLevelFactory.parameters.Speed);
        }
    }

    public override string GetSequentialHiddenData()
    {
        return WagonsManager.Instance.hiddenDataEncoder.EncodingSequentialData();
    }

    public override string GetCommonHiddenData()
    {
        return WagonsManager.Instance.hiddenDataEncoder.EncodingCommonData();
    }

    #endregion

    #region Calculating Parameters

    public override void GetParameters()
    {
        myLevelFactory.GetParameters();
    }

    #endregion

    #region Buttons

    public void WasButton_CLicked()
    {
        if (WagonsManager.Instance.ActiveButton && LevelFactory.Instance.CurrentState == LevelDiffStates.WithoutChange)
        {
            if (WagonsManager.Instance.IsTrueWagon())
                EventManager.Instance.InvokeEvent("TrueAnswerEvent");
            else
                EventManager.Instance.InvokeEvent("FalseAnswerEvent");
            WagonsManager.Instance.EnableActiveButton(false);
        }

    }

    public void ReadyButton_CLicked()
    {
        WagonsManager.Instance.StartMovingTrain();
        Timers.Instance.StartTimer("ResponseTimer", 0);
        Timers.Instance.StartAnswerTimer();
    }

    #endregion

    #region Tutorial Handling
    internal override Vector3 GetAnswerPosition()
    {
        GameObject readybtn = GameObject.Find("ReadyButton");
        if (readybtn != null)
        {
            return new Vector3(0, 0, 10);
        }
        if (WagonsManager.Instance.ActiveButton)
            if (WagonsManager.Instance.IsTrueWagon())
            {
                GameObject temp = GameObject.Find("wasButton");
                return temp.transform.parent.localPosition + temp.transform.parent.transform.parent.localPosition + temp.transform.localPosition;
            }
        return new Vector3(0, 0, 10); 

    }

    #endregion
}
