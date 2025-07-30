using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighwayLevelFactory : LevelFactory
{
    public HighwayParameters parameters = new HighwayParameters();
    public HighwayLevelFactory()
    {
        LevelNumber = 24;
        WithoutLevelChange = true;
    }

    override public void GetParameters()
    {
        switch (CurrentLevel)
        {
            case 1:
                parameters.SetLevelParameters(3, 1, 1, 1);
                break;
            case 2:
                parameters.SetLevelParameters(3, 2, 1, 2);
                break;
            case 3:
                parameters.SetLevelParameters(3, 3, 1, 2);
                break;
            case 4:
                parameters.SetLevelParameters(4, 3, 1, 2);
                break;
            case 5:
                parameters.SetLevelParameters(3, 1, 2, 1);
                break;
            case 6:
                parameters.SetLevelParameters(3, 2, 2, 1);
                break;
            case 7:
                parameters.SetLevelParameters(3, 3, 2, 1);
                break;
            case 8:
                parameters.SetLevelParameters(4, 3, 2, 1);
                break;
            case 9:
                parameters.SetLevelParameters(3, 1, 2, 2);
                break;
            case 10:
                parameters.SetLevelParameters(3, 2, 2, 2);
                break;
            case 11:
                parameters.SetLevelParameters(3, 3, 2, 2);
                break;
            case 12:
                parameters.SetLevelParameters(4, 1, 2, 2);
                break;
            case 13:
                parameters.SetLevelParameters(4, 2, 2, 2);
                break;
            case 14:
                parameters.SetLevelParameters(4, 3, 2, 2);
                break;
            case 15:
                parameters.SetLevelParameters(3, 1, 1, 3);
                break;
            case 16:
                parameters.SetLevelParameters(3, 2, 1, 3);
                break;
            case 17:
                parameters.SetLevelParameters(3, 3, 1, 3);
                break;
            case 18:
                parameters.SetLevelParameters(4, 3, 1, 3);
                break;
            case 19:
                parameters.SetLevelParameters(3, 1, 2, 3);
                break;
            case 20:
                parameters.SetLevelParameters(3, 2, 2, 3);
                break;
            case 21:
                parameters.SetLevelParameters(3, 3, 2, 3);
                break;
            case 22:
                parameters.SetLevelParameters(4, 1, 2, 3);
                break;
            case 23:
                parameters.SetLevelParameters(4, 2, 2, 3);
                break;
            case 24:
                parameters.SetLevelParameters(4, 3, 2, 3);
                break;
        }
        switch (CurrentDifficulty)
        {
            case 0:
                parameters.SetDifficultyParameters(1f + (1f * 2f / 24f), 4f);
                break;

            case 1:
                parameters.SetDifficultyParameters(2f + (1f * 2f / 24f), 3f);
                break;

            case 2:
                parameters.SetDifficultyParameters(3f + (1f * 2f / 24f), 2f);
                break;

            case 3:
                parameters.SetDifficultyParameters(4f + (1f * 2f / 24f), 1f);
                break;

            default:
                parameters.SetDifficultyParameters(4f + (1f * 2f / 24f), 1f);
                break;
        }
    }
}
