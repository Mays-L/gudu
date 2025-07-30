using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ScoreBoard : MonoBehaviour
{
    #region Fields

    /// <summary>
    /// Score
    /// </summary>
    private int score = 0;
    public int Score { get { return score; } }

    /// <summary>
    /// Game deuration
    /// </summary>
    private int gameTime = 90;
    public int GameTime { get { return gameTime; } }

    /// <summary>
    /// Current level
    /// </summary>
    private int level = 1;
    public int Level { get { return level; } }

    /// <summary>
    /// Number of game levels
    /// </summary>
    private int numberOfLevels = 1;
    public int NumberOfLevels { get { return numberOfLevels; } }

    /// <summary>
    /// Number of answers in round
    /// </summary>
    private int answersNumber = 3;
    public int AnswersNumber { get { return answersNumber; } }

    /// <summary>
    /// True answers number in round
    /// </summary>
    private int trueAnswersNumber = 2;
    public int TrueAnswersNumber { get { return trueAnswersNumber; } }

    /// <summary>
    /// Number of clicked or answers in round
    /// </summary>
    private int allUserAnswersNumber = 0;
    public int AllUserAnswers { get { return allUserAnswersNumber; } }

    /// <summary>
    /// Secound counter
    /// </summary>
    private double SecondCounter = 1;

    /// <summary>
    /// Value of each true answer
    /// </summary>
    private readonly int ValueOfTrueAnswer = 100;

    /// <summary>
    /// Remain time
    /// </summary>
    private int remainTime = 90;
    public int RemainTime { get { return remainTime; } }

    private GameObject EndDialog;
    //private SoundEffectHandler _SoundEffectHandler;
    GameObject timeObj;
    GameObject scoreObj;
    GameObject timeBarObj;
    GameObject levelObj;
    GameObject levelNumberObj;
    GameObject progressObj;
    GameObject progressBarObj;
    #endregion

    #region Methods


    /// <summary>
    /// Unity start method
    /// </summary>
    void Start()
    {
        remainTime = gameTime;
        UpdateUI();
        EndDialog = Resources
                .FindObjectsOfTypeAll<GameObject>()
                .FirstOrDefault(g => g.CompareTag("EndCanvas"));
        EndDialog.SetActive(false);
        InitializeUI();

    }

    public void InitializeUI()
    {

        timeObj = GameObject.Find("Time");
        timeBarObj = GameObject.Find("FillBarTime");
        levelObj = GameObject.Find("Level");
        levelNumberObj = GameObject.Find("LevelNumber");
        progressObj = GameObject.Find("Progress");
        progressBarObj = GameObject.Find("FillBarProgress");
        scoreObj = GameObject.Find("Score");
    }

    /// <summary>
    /// Unity update method
    /// </summary>
    void Update()
    {
        SecondCounter -= Time.deltaTime;
        if (SecondCounter <= 0)
        {
            if (remainTime > 0)
            {
                DiscountTime();
                SetTime(gameTime, remainTime);
                if (remainTime <= 5)
                {
                    EventManager.Instance.InvokeEvent("LastSecondsEvent");
                }
            }
            else
            {
                EventManager.Instance.InvokeEvent("FinishedGameEvent");
            }
            SecondCounter = 1;
        }
        if (progressObj == null)
            InitializeUI();

    }

    /// <summary>
    /// Discount time
    /// </summary>
    private void DiscountTime()
    {
        remainTime -= 1;
    }

    /// <summary>
    /// Add Score
    /// </summary>
    public void AddScore()
    {
        trueAnswersNumber += 1;
        score += ValueOfTrueAnswer;
        SetScore(score);
        SetProgress(trueAnswersNumber, answersNumber);
    }

    /// <summary>
    /// Get round performance
    /// </summary>
    /// <returns>Performance between 0 to 1</returns>
    internal float GetRoundPerformance()
    {
        return (float)TrueAnswersNumber / allUserAnswersNumber;
    }

    /// <summary>
    /// Add clicked numbers
    /// </summary>
    internal void AddUserAnswer()
    {
        allUserAnswersNumber++;
    }


    /// <summary>
    /// Set round parameters called by game handler 
    /// </summary>
    /// <param name="level">Level</param>
    /// <param name="numberOfLevels">Number of levels</param>
    /// <param name="answersNumber">Number of true answers</param>
    public void SetRoundParameters(int level, int numberOfLevels, int answersNumber)
    {
        this.level = level;
        this.numberOfLevels = numberOfLevels;
        this.answersNumber = answersNumber;
        trueAnswersNumber = 0;
        SecondCounter = 1;
        allUserAnswersNumber = 0;
        UpdateUI();
    }

    /// <summary>
    /// Set time
    /// </summary>
    /// <param name="time"></param>
    public void SetTime(int time)
    {
        gameTime = remainTime = time;
        SetTime(time, time);
    }


    /// <summary>
    /// Update ui values
    /// </summary>
    public void UpdateUI()
    {
        SetLevelNumber(numberOfLevels);
        SetLevel(level);
        SetProgress(trueAnswersNumber, answersNumber);
        SetScore(score);
        SetTime(gameTime, remainTime);
    }

    /// <summary>
    /// Update level in ui
    /// </summary>
    /// <param name="level">level</param>
    public void SetLevel(int _level)
    {
        level = _level;
        if(levelObj!=null)
        levelObj.GetComponent<Text>().text = level.ToString();
    }

    /// <summary>
    /// Update number of levels in UI
    /// </summary>
    /// <param name="level">level</param>
    public void SetLevelNumber(int _level)
    {
        numberOfLevels =  _level;
        if (levelNumberObj != null)
            levelNumberObj.GetComponent<Text>().text = "/" + numberOfLevels.ToString();
    }

    /// <summary>
    /// Set score in UI
    /// </summary>
    /// <param name="score">score int</param>
    public void SetScore(int _score)
    {
        score = _score;
        if(scoreObj!=null)
            scoreObj.GetComponent<Text>().text = score.ToString();
    }

    /// <summary>
    /// Update progess in UI
    /// </summary>
    /// <param name="trueAns">True answers number</param>
    /// <param name="ansNum">Number of answers</param>
    public void SetProgress(int trueAns, int ansNum)
    {
        trueAnswersNumber = trueAns;
        answersNumber = ansNum;
        allUserAnswersNumber = answersNumber;

        if (progressObj != null)
        {
            progressObj.GetComponent<Text>().text = trueAns + "/" + ansNum;
            if (ansNum == 0)
                ansNum = 1;
            progressBarObj.GetComponent<Image>().fillAmount = (float)trueAns / ansNum;

        }
    }

    /// <summary>
    /// Update time in ui
    /// </summary>
    /// <param name="time">Game deuration</param>
    /// <param name="remainTime">Remain time</param>
    public void SetTime(int _gametime, int _remainTime)
    {
        if(timeBarObj==null)
        {
            timeObj = GameObject.Find("Time");
            timeBarObj = GameObject.Find("FillBarTime");
        }
        remainTime = _remainTime;
        gameTime = _gametime;
        try
        {
            timeObj.GetComponent<Text>().text = remainTime + "/" + gameTime;
            timeBarObj.GetComponent<Image>().fillAmount = (float)remainTime / gameTime;
        }
        catch { }
    }
    #endregion
}

