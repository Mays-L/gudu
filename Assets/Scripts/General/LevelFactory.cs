using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum LevelDiffStates
{
    WithoutChange, DifficultyChanged, LevelChanged
}
public class LevelFactory: Singleton<LevelFactory>
{
    public List<DetailParams> RequiredLevels;

    protected int RepeatNum = 5;
    protected float lowerPerf = 0.2f, higherPerf = 0.8f;
    protected bool WithoutLevelChange = false;
    public int LevelNumber { get; protected set; } = 1;
    public int CurrentLevel { get; protected set; } = 1;
    public int CurrentDifficulty { get; protected set; } = 0;
    public LevelDiffStates CurrentState { get; protected set; } = LevelDiffStates.WithoutChange;
    protected List<bool> DifficultyPerforms = new List<bool>(), LevelPerforms = new List<bool>();
    private int ResponseCounter = 0;
    private int NewLevel = 1, NewDifficulty = 0;
    private bool levelsSet = false;

    public void SetLevelsParameters(GameParameters _gp)
    {
        if (levelsSet)
            return;
        RequiredLevels = new List<DetailParams>();
        RequiredLevels = _gp.AdvancedSettings.ToList<DetailParams>();
        SetInitLevelDiff(RequiredLevels[0].Level, 0);
        levelsSet = true;
    }

    public virtual void GetParameters()
    {

    }

    void LevelDifficultyControllerRehabWithoutLevelChange(bool _perform)
    {
        CurrentState = LevelDiffStates.WithoutChange;
        DifficultyPerforms.Add(_perform);

        if (DifficultyPerforms.Count >= RepeatNum)
        {
            float diffPerform = 0f;
            foreach (bool _b in DifficultyPerforms)
            {
                if (_b)
                    diffPerform = diffPerform + 1f;
            }
            diffPerform /= DifficultyPerforms.Count;

            if (diffPerform >= higherPerf)
            {
                NewDifficulty = (CurrentDifficulty >= 3) ? 3 : CurrentDifficulty + 1;
                CurrentState = LevelDiffStates.DifficultyChanged;
            }
            else if (diffPerform <= lowerPerf)
            {
                NewDifficulty = (CurrentDifficulty <= 0) ? 0 : CurrentDifficulty - 1;
                CurrentState = LevelDiffStates.DifficultyChanged;
            }

            DifficultyPerforms.Clear();
        }
    }

    void LevelDifficultyControllerRehab(bool _perform)
    {
        CurrentState = LevelDiffStates.WithoutChange;
        DifficultyPerforms.Add(_perform);

        if (DifficultyPerforms.Count >= RepeatNum)
        {
            float diffPerform = 0f;
            foreach (bool _b in DifficultyPerforms)
            {
                if (_b)
                    diffPerform = diffPerform + 1f;
            }
            diffPerform /= DifficultyPerforms.Count;

            if (diffPerform >= higherPerf)
            {
                CurrentState = LevelDiffStates.DifficultyChanged;
                NewDifficulty = CurrentDifficulty + 1;
                if (NewDifficulty > 3)
                {
                    NewDifficulty = 0;
                    NewLevel = (CurrentLevel >= LevelNumber) ? LevelNumber : CurrentLevel + 1;
                    CurrentState = LevelDiffStates.LevelChanged;
                    LevelPerforms.Clear();
                }
                else
                    LevelPerforms.Add(true);
            }
            else if (diffPerform <= lowerPerf)
            {
                CurrentState = LevelDiffStates.DifficultyChanged;
                NewDifficulty = CurrentDifficulty - 1;
                if (NewDifficulty < 0)
                {
                    NewDifficulty = 3;
                    NewLevel = (CurrentLevel <= 0) ? 0 : CurrentLevel - 1;
                    CurrentState = LevelDiffStates.LevelChanged;
                    LevelPerforms.Clear();
                }
            }
            else
                LevelPerforms.Add(false);

            if (LevelPerforms.Count >= 4)
            {
                if (LevelPerforms.Contains(true))
                {
                    NewDifficulty = 0;
                    NewLevel = (CurrentLevel >= LevelNumber) ? LevelNumber : CurrentLevel + 1;
                    CurrentState = LevelDiffStates.LevelChanged;
                    LevelPerforms.Clear();
                }
            }

            DifficultyPerforms.Clear();
        }
    }

    void LevelDifficultyControllerAssess(bool _perform)
    {
        ResponseCounter++;
        if (RequiredLevels.Count > 0)
        {
            if (ResponseCounter >= RequiredLevels[0].Repeat)
            {
                ResponseCounter = 0;
                RequiredLevels.RemoveAt(0);
                if (RequiredLevels.Count > 0)
                {
                    NewLevel = RequiredLevels[0].Level;
                    NewDifficulty = 0;
                    LevelPerforms.Clear();
                    DifficultyPerforms.Clear();
                }
                else
                    SetZeroTime();

                CurrentState = LevelDiffStates.LevelChanged;
            }
        }
        else
        {
            SetZeroTime();
        }
    }

    void SetZeroTime()
    {
        ScoreBoard obj = GameObject.FindObjectOfType<ScoreBoard>();
        if (obj.GameTime > 1)
        {
            obj.SetTime(1); 
            Timers.Instance.StartTimer(1f, FinishGame);
        }
    }

    void FinishGame()
    {
        EventManager.Instance.InvokeEvent("FinishedGameEvent");
    }

    public void LevelDifficultyController(bool _perform)
    {
        if (WithoutLevelChange)
            LevelDifficultyControllerRehabWithoutLevelChange(_perform);
        else
            LevelDifficultyControllerRehab(_perform);
        if (GameManager.Instance.RunType == 0)
            LevelDifficultyControllerAssess(_perform);
    }

    public void UpdateLevelDifficulty()
    {
        CurrentDifficulty = NewDifficulty;
        CurrentLevel = NewLevel;
        CurrentState = LevelDiffStates.WithoutChange;
        GetParameters();
    }

    public void SetInitLevelDiff(int _level, int _diff)
    {
        CurrentDifficulty = _diff;
        NewDifficulty = CurrentDifficulty;
        CurrentLevel = _level;
        NewLevel = CurrentLevel;
        GetParameters();
    }
}
