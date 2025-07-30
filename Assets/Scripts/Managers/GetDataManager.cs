using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDataManager : Singleton<GetDataManager>
{
    public GameParameters GameParameters = new GameParameters();
    public void SetGameParameters(string parameters)
    {
        GameParameters = JsonUtility.FromJson<GameParameters>(parameters);
        if (!GameParameters.ResultsHost.Equals("") && GameParameters.ResultsHost != null)
            SendDataManager.Instance.Url = GameParameters.ResultsHost;
        LevelFactory.Instance.SetLevelsParameters(GameParameters);
    }
}

[Serializable]
public class GameParameters
{
    public string ResultsHost;
    public long UserAssignmentId;
    public int RunType; // 0: Assessment / 1: Game / 2: PerfectGamer
    public DetailParams[] AdvancedSettings;
    public string RedirectUrl;
}

[Serializable]
public struct DetailParams
{
    public int Distractor;
    public int Level;
    public int Repeat;
    public int Time;
}