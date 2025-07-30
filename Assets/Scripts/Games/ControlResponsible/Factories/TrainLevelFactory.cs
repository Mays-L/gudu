using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainLevelFactory : LevelFactory
{
    public TrainParameters parameters = new TrainParameters();
    public TrainLevelFactory()
    {
        LevelNumber = 29;
        WithoutLevelChange = true;
    }

    public override void GetParameters()
    {
        int q;
        switch (CurrentLevel)
        {
            case 1:
                q = 1;
                parameters.SetLevelParameters(q, 3 * q);
                break;
            case 2:
                q = 1;
                parameters.SetLevelParameters(q, 4 * q);
                break;
            case 3:
                q = 1;
                parameters.SetLevelParameters(q, 5 * q);
                break;
            case 4:
                q = 1;
                parameters.SetLevelParameters(q, 6 * q);
                break;
            case 5:
                q = 1;
                parameters.SetLevelParameters(q, 7 * q);
                break;
            case 6:
                q = 2;
                parameters.SetLevelParameters(q, 2 * q);
                break;
            case 7:
                q = 2;
                parameters.SetLevelParameters(q, 3 * q);
                break;
            case 8:
                q = 2;
                parameters.SetLevelParameters(q, 4 * q);
                break;
            case 9:
                q = 2;
                parameters.SetLevelParameters(q, 5 * q);
                break;
            case 10:
                q = 2;
                parameters.SetLevelParameters(q, 6 * q);
                break;
            case 11:
                q = 2;
                parameters.SetLevelParameters(q, 7 * q);
                break;
            case 12:
                q = 3;
                parameters.SetLevelParameters(q, 2 * q);
                break;
            case 13:
                q = 3;
                parameters.SetLevelParameters(q, 3 * q);
                break;
            case 14:
                q = 3;
                parameters.SetLevelParameters(q, 4 * q);
                break;
            case 15:
                q = 3;
                parameters.SetLevelParameters(q, 5 * q);
                break;
            case 16:
                q = 3;
                parameters.SetLevelParameters(q, 6 * q);
                break;
            case 17:
                q = 3;
                parameters.SetLevelParameters(q, 7 * q);
                break;
            case 18:
                q = 4;
                parameters.SetLevelParameters(q, 2 * q);
                break;
            case 19:
                q = 4;
                parameters.SetLevelParameters(q, 3 * q);
                break;
            case 20:
                q = 4;
                parameters.SetLevelParameters(q, 4 * q);
                break;
            case 21:
                q = 4;
                parameters.SetLevelParameters(q, 5 * q);
                break;
            case 22:
                q = 4;
                parameters.SetLevelParameters(q, 6 * q);
                break;
            case 23:
                q = 4;
                parameters.SetLevelParameters(q, 7 * q);
                break;
            case 24:
                q = 5;
                parameters.SetLevelParameters(q, 2 * q);
                break;
            case 25:
                q = 5;
                parameters.SetLevelParameters(q, 3 * q);
                break;
            case 26:
                q = 5;
                parameters.SetLevelParameters(q, 4 * q);
                break;
            case 27:
                q = 5;
                parameters.SetLevelParameters(q, 5 * q);
                break;
            case 28:
                q = 5;
                parameters.SetLevelParameters(q, 6 * q);
                break;
            case 29:
                q = 5;
                parameters.SetLevelParameters(q, 7 * q);
                break;
        }
        switch (CurrentDifficulty)
        {
            case 0:
                parameters.SetDifficultyParameters(1f + 2f * CurrentLevel / 10f);
                break;

            case 1:
                parameters.SetDifficultyParameters(2f + 2f * CurrentLevel / 10f);
                break;

            case 2:
                parameters.SetDifficultyParameters(3f + 2f * CurrentLevel / 10f);
                break;

            case 3:
                parameters.SetDifficultyParameters(4f + 2f * CurrentLevel / 10f);
                break;

            default:
                parameters.SetDifficultyParameters(4f + 2f * CurrentLevel / 10f);
                break;
        }
    }
}
