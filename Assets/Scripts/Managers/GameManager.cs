using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GameManager : Singleton<GameManager>
{
    #region Game States Variables
    // All the states in the game
    public enum GameState
    {
        START,
        RUNNING,
        FINISHED
    }

    //If the player was itimn the same state these bool variables 
    protected bool _startState = false;
    protected bool _runningState = false;
    protected bool _finishedState = false;


    // Game States UI
    public GameObject[] startUIs;
    public GameObject[] runningUIs;
    public GameObject[] finishedUIs;
    public GameObject Tutorial;

    // The state that we currently are in
    public GameState CurrentState { get; protected set; } = GameState.START;

    public int RunType = 1; // 0: Assessment, 1: Game, 2: PerfectGamer
    public int GameTime = 90;
    public int InitLevel = 1;
    protected bool ShowLocked = false;
    protected float BetweenLevelsTime = 0.1f;
    protected int trueAnswerScore = 100;
    protected int repeat = 1;
    internal bool FirstEnter = true;
    protected bool IsHideLevel = false;
    protected bool TutorialDialog = false;

    internal bool isInTutorial = true;

    // Data variables
    internal List<PlayerLevelData> playerLevelDataList;

    internal PlayerGameData playerGameData;

    //UI
    [SerializeField]
    protected ScoreBoard scoreBoard;
    [SerializeField]
    EndDialog endDialog;

    public GameObject levelSliderGameObjcet;
    protected Slider levelSlider;
    public GameObject levelTextGameObject;
    protected Text levelText;
    public Type objectManagerType;
   

    #endregion

    protected virtual void Start()
    {
        EventManager.Instance.Initialize();

        #region Initialization
        //Level Slider
        if (levelSliderGameObjcet != null) levelSlider = levelSliderGameObjcet.GetComponent<Slider>();
#if UNITY_EDITOR
        levelSlider.interactable = true;
#endif

        //LevelText
        if (levelTextGameObject != null) levelText = levelTextGameObject.GetComponent<Text>();

        //Player Data
        repeat = 1;
        playerLevelDataList = new List<PlayerLevelData>();
        #endregion

        Add_Events_Listeners();
        SetTimeAndLevel();
        Get_Send_GameData(false);
        ProcessGameStates();
    }
    protected virtual void Update()
    {
        if (levelSlider != null && levelText != null && CurrentState == GameState.START)
        {
            levelText.text = Convert.ToInt32(levelSlider.value).ToString() + "/" + LevelFactory.Instance.LevelNumber;
            GetLevel();
        }
    }

    /// <summary>
    /// Adding Events and listeners for this game
    /// </summary>
    protected virtual void Add_Events_Listeners()
    {
        #region Adding The Events

        //When the player choose the correct or wrong color these event will be invoked respectedly.
        EventManager.Instance.AddNoParameterEvent("TrueAnswerEvent");
        EventManager.Instance.AddNoParameterEvent("FalseAnswerEvent");

        //When the game is ended, this event will be invoked
        EventManager.Instance.AddNoParameterEvent("FinishedGameEvent");
        EventManager.Instance.AddNoParameterEvent("LastSecondsEvent");

        //When the game is strted this event will be invoked
        EventManager.Instance.AddNoParameterEvent("StartGameEvent");
        //When the game restart or home button clicked
        EventManager.Instance.AddNoParameterEvent("RestartGameEvent");


        #endregion

        #region Add Listeners
        EventManager.Instance.AddListeners("TrueAnswerEvent", AddToTrueAnswers);
        EventManager.Instance.AddListeners("FalseAnswerEvent", AddToFalseAnswers);
        EventManager.Instance.AddListeners("FinishedGameEvent", FinishedGame);
        #endregion
    }

    #region GameState
    /// <summary>
    /// Changing the UI and other settings when according to each state that we're in.
    /// </summary>
    public virtual void ProcessGameStates()
    {
        switch (CurrentState)
        {
            case GameState.START:
                if (!_startState)
                {
                    foreach (GameObject startUI in startUIs) startUI.SetActive(true);
                    foreach (GameObject runningUI in runningUIs) runningUI.SetActive(false);
                    foreach (GameObject finishUI in finishedUIs) finishUI.SetActive(false);
                    _runningState = false;
                    _finishedState = false;
                    _startState = true;
                }

                break;

            case GameState.RUNNING:
                if (!_runningState && !TutorialDialog)
                {
                    foreach (GameObject startUI in startUIs) startUI.SetActive(false);
                    foreach (GameObject runningUI in runningUIs) runningUI.SetActive(true);
                    foreach (GameObject finishUI in finishedUIs) finishUI.SetActive(false);
                    RunTutorial();
                    SetTime();
                    _runningState = true;
                    _startState = false;
                    _finishedState = false;
                    Timers.Instance.StartTimer("LevelTimer", 0f);
                }
                GetParameters();
                Timers.Instance.StartTimer(0.1f,UpdateResultsOnScoreBoard);
                Timers.Instance.StartTimer(BetweenLevelsTime, ShowLevel);
                
                break;
            case GameState.FINISHED:
                if (!_finishedState)
                {
                    foreach (GameObject startUI in startUIs) startUI.SetActive(false);
                    foreach (GameObject runningUI in runningUIs) runningUI.SetActive(false);
                    foreach (GameObject finishUI in finishedUIs) finishUI.SetActive(true);
                    _finishedState = true;
                    _startState = false;
                    _runningState = false;
                }

                SetReport();
                break;
        }
    }


    public void PauseGame()
    {
        Time.timeScale = 0;
        DisableUIElements();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        IsHideLevel = false;
        ProcessGameStates();
    }

    public void DisableUIElements()
    {
        foreach (GameObject startUI in startUIs) startUI.SetActive(false);
        foreach (GameObject runningUI in runningUIs) runningUI.SetActive(false);
        foreach (GameObject finishUI in finishedUIs) finishUI.SetActive(false);
        _runningState = false;
        _finishedState = false;
        _startState = false;
    }
    #endregion
    private void SetTimeAndLevel()
    {
#if UNITY_EDITOR
        RunType = 1;
        string tempJson;
        if (RunType == 0)
        {
            tempJson = "{" +
                "\"ResultsHost\" : null," +
                "\"UserAssignmentId\" : 0," +
                "\"RunType\" : 0," +
                "\"AdvancedSettings\": [{\"Distractor\":0,\"Level\":2,\"Repeat\":20,\"Time\":90},{\"Distractor\":5,\"Level\":3,\"Repeat\":3,\"Time\":90},{\"Distractor\":5,\"Level\":1,\"Repeat\":3,\"Time\":90}]," +
                "\"RedirectUrl\" : \"\"}";
        }
        else
        {
            tempJson = "{" +
                "\"ResultsHost\" : null," +
                "\"UserAssignmentId\" : 0," +
                "\"RunType\" : 1," +
                "\"AdvancedSettings\": [{\"Distractor\":0,\"Level\":2,\"Repeat\":0,\"Time\":900}]," +
                "\"RedirectUrl\" : \"\"}";
        }

        GetDataManager.Instance.SetGameParameters(tempJson);

#elif UNITY_WEBGL
            while (GetDataManager.Instance.GameParameters.ResultsHost==null || GetDataManager.Instance.GameParameters.ResultsHost.Equals("")) ;
#endif

        RunType = GetDataManager.Instance.GameParameters.RunType;
        InitLevel = GetDataManager.Instance.GameParameters.AdvancedSettings[0].Level;
        GameTime = 0;
        foreach (DetailParams data in GetDataManager.Instance.GameParameters.AdvancedSettings)
        {
            GameTime += data.Time;
        }
        levelSlider.value = InitLevel;
    }

    #region Gathering and Saving Players Data
    /// <summary>
    /// Get level data for each level
    /// </summary>
    void GetLevelData()
    {
        PlayerLevelData newPlayerLevelData = new PlayerLevelData();
        newPlayerLevelData.LevelNumber = LevelFactory.Instance.CurrentLevel;
        newPlayerLevelData.TrueAnswerNumber = ResultsHandling.Instance.LevelTrueAnswersCounter;
        newPlayerLevelData.FalseAnswerNumber = ResultsHandling.Instance.LevelFalseAnswersCounter;
        newPlayerLevelData.LevelNumber = LevelFactory.Instance.CurrentLevel;
        newPlayerLevelData.TimeToPass = Timers.Instance.StopTimer("LevelTimer");
        playerLevelDataList.Add(newPlayerLevelData);
        Timers.Instance.StartTimer("LevelTimer", 0f);
    }

    internal void SetLevel(int level)
    {
        levelSlider.value = level;
        LevelFactory.Instance.SetInitLevelDiff(level, 0);
    }

    /// <summary>
    /// Get and send Game Data
    /// </summary>
    internal void Get_Send_GameData(bool finished)
    {
        GetLevelData();
        PlayerLevelData lastData = playerLevelDataList[playerLevelDataList.Count - 1];
        GameResults gr = new GameResults(lastData.TrueAnswerNumber,
            lastData.FalseAnswerNumber,
            ResultsHandling.Instance.Score,
            finished,
            (int)lastData.TimeToPass,
            ResultsHandling.Instance.GetDetails(),
            LevelFactory.Instance.CurrentLevel
            , repeat
            , ResultsHandling.Instance.TrueFalseSequence
            , ResultsHandling.Instance.AnswerTimeSequence
            , Screen.width
            , Screen.height);
        if (GetDataManager.Instance != null && GetDataManager.Instance.GameParameters.UserAssignmentId != 0)
            gr.UserAssignmentId = GetDataManager.Instance.GameParameters.UserAssignmentId;
        SendDataManager.Instance.SendGameResults(gr);
    }
    #endregion

    #region Shifting the states
    protected void GoToStartState()
    {
        ResultsHandling.Instance.ResetScore();
        CurrentState = GameState.START;
        ProcessGameStates();
    }
    public void GoToRunningState()
    {
        CurrentState = GameState.RUNNING;
        EventManager.Instance.InvokeEvent("StartGameEvent");
        Timers.Instance.StartTimer(0.25f, ProcessGameStates);
    }
    
    internal bool IsRunningState()
    {
        return _runningState;
    }
    protected void GoToFinishedState()
    {
        CurrentState = GameState.FINISHED;
        ProcessGameStates();
    }
    #endregion

    #region Buttons
    /// <summary>
    /// Go to sart game state
    /// </summary>
    public virtual void ReloadGame()
    {
        EventManager.Instance.InvokeEvent("RestartGameEvent");
        CurrentState = GameState.START;
        _runningState=false;
        ResultsHandling.Instance.ResetScore();
        HideLevel();
        repeat++;
    }

    public virtual void RestartLevel()
    {
        SetLevel(LevelFactory.Instance.CurrentLevel);
        ResultsHandling.Instance.ResetScore();
        playerLevelDataList.Clear();
        _runningState = false;
        TutorialDialog = false;
        GoToRunningState();
    }

    /// <summary>
    /// Send data and reset level
    /// </summary>
    public virtual void ResetLevel()
    {
        playerLevelDataList.Clear();
        GameTime = scoreBoard.GameTime;
        ResultsHandling.Instance.ResetScore();
        HideLevel();
        repeat++;
    }
    #endregion

    protected void FinishedGame()
    {
        if (FinishedTutorial())
            Get_Send_GameData(true);
        HideLevel();
        GoToFinishedState();
    }

    protected void SetReport()
    {
        ResultsHandling result = ResultsHandling.Instance;
        float truePercent = 0, trueNum = 0, falseNum = 0;
        foreach (PlayerLevelData pld in playerLevelDataList)
        {
            trueNum += pld.TrueAnswerNumber;
            falseNum += pld.FalseAnswerNumber;
        }
        if (trueNum + falseNum > 0)
        {
            truePercent = (float)trueNum / (trueNum + falseNum);
        }
        endDialog.SetUIValues(result.GetPerformance(), result.Score
            , truePercent, result.GetProgress());
    }

    protected virtual void UpdateResultsOnScoreBoard()
    {
        scoreBoard.SetLevelNumber(LevelFactory.Instance.LevelNumber);
        scoreBoard.SetLevel(LevelFactory.Instance.CurrentLevel);
        scoreBoard.SetProgress(ResultsHandling.Instance.LevelTrueAnswersCounter, ResultsHandling.Instance.AllAnswers);
        scoreBoard.SetScore(ResultsHandling.Instance.Score);
        
    }

    protected void SetTime()
    {
        scoreBoard.SetTime(GameTime);
    }

    #region Level handling

    public virtual void HideLevel()
    {
        IsHideLevel = true;
    }

    public virtual void ShowLevel()
    {
        IsHideLevel = false;
    }

    #endregion


    #region Processing Results

    protected virtual void AddToTrueAnswers()
    {
        ResultsHandling.Instance.AddToTrueSelections();
        AddToScore(trueAnswerScore);
        UpdateResultsOnScoreBoard();
        AddToResponseTimes();
        CheckEndOfLevel();
    }

    protected virtual void AddToFalseAnswers()
    {
        ResultsHandling.Instance.AddToFalseSelections();
        AddToScore(-trueAnswerScore / 2);
        UpdateResultsOnScoreBoard();
        AddToResponseTimes();
        CheckEndOfLevel();
    }

    protected virtual void AddToScore(int amountOfScore)
    {
        int m = (int)Math.Ceiling((float)LevelFactory.Instance.CurrentLevel * 5 / LevelFactory.Instance.LevelNumber);
        ResultsHandling.Instance.AddToScore(amountOfScore * m);
    }

    protected void AddToResponseTimes()
    {
        float timer = Timers.Instance.GetTimer("ResponseTimer");
        int timer2 = (int)(Timers.Instance.GetAnswerTimer());
        ResultsHandling.Instance.AddToResponseTimes((int)(timer * 1000), timer2);
    }


    /// <summary>
    /// Used to return the encoded sequential data (maybe overrided in the child class)
    /// </summary>
    public virtual string GetSequentialHiddenData()
    {
        return null;
    }

    /// <summary>
    /// Used to return the encoded common data (maybe overrided in the child class)
    /// </summary>
    public virtual string GetCommonHiddenData()
    {
        return null;
    }

    protected virtual void CheckEndOfLevel()
    {
        Timers.Instance.StartTimer(1f, HideLevel);
    }
    #endregion

    #region Calculating Parameters

    public abstract void GetParameters();

    protected void GetLevel()
    {
        if (levelSlider != null)
            InitLevel = Convert.ToInt32(levelSlider.value);
        else InitLevel = 0;
        LevelFactory.Instance.SetInitLevelDiff(InitLevel, 0);
    }

    #endregion

    #region Tutorial Handling
    internal virtual Vector3 GetAnswerPosition()
    {
        return new Vector3();
    }

    internal virtual void RunTutorial()
    {
        if (FirstEnter)
        {
            Tutorial.SetActive(true);
        }
        else
        {
            Tutorial.SetActive(false);
        }
    }
    internal virtual bool FinishedTutorial()
    {
        if (ResultsHandling.Instance.GameTrueAnswerCounter >= 2 || !isInTutorial || RunType == 2)
        {
            return true;
        }
        return false;
    }
    internal virtual void EndOfTutorial()
    {
        if (!IsHideLevel)
            HideLevel();
        FirstEnter = false;
        playerLevelDataList.Clear();
        GameTime = scoreBoard.GameTime;
        scoreBoard.SetTime(GameTime);
        ResultsHandling.Instance.ResetScore();
    }

    internal virtual bool IsInHideLevel()
    {
        return IsHideLevel;
    }
    #endregion
}
