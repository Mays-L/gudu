using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level factory
/// </summary>
public class CloudsLevelFactory: LevelFactory
{
    public CloudsParameters parameters = new CloudsParameters();
    public CloudsLevelFactory()
    {
        LevelNumber = 29;
    }

    public override void GetParameters()
    {

        switch (CurrentLevel)
        {
            case 1:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 2, 1);
                break;
            case 2:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 2, 2);
                break;
            case 3:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 3, 1);
                break;
            case 4:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 3, 2);
                break;
            case 5:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 3, 3);
                break;
            case 6:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 4, 1);
                break;
            case 7:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 4, 2);
                break;
            case 8:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 4, 3);
                break;
            case 9:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 4, 4);
                break;
            case 10:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 5, 1);
                break;
            case 11:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 5, 2);
                break;
            case 12:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 5, 3);
                break;
            case 13:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 5, 4);
                break;
            case 14:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 6, 1);
                break;
            case 15:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 6, 2);
                break;
            case 16:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 6, 3);
                break;
            case 17:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 6, 4);
                break;
            case 18:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 7, 1);
                break;
            case 19:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 7, 2);
                break;
            case 20:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 7, 3);
                break;
            case 21:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 7, 4);
                break;
            case 22:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 8, 1);
                break;
            case 23:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 8, 2);
                break;
            case 24:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 8, 3);
                break;
            case 25:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 8, 4);
                break;
            case 26:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 9, 1);
                break;
            case 27:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 9, 2);
                break;
            case 28:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 9, 3);
                break;
            case 29:
                parameters.SetLevelParameters((int)Math.Ceiling((double)(CurrentLevel * 3) / 5) + 11, 9, 4);
                break;
        }

        switch (CurrentDifficulty)
        {
            case 0:
                parameters.SetDifficultyParameters(300, 30);
                break;

            case 1:
                parameters.SetDifficultyParameters(450, 45);
                break;

            case 2:
                parameters.SetDifficultyParameters(600, 60);
                break;

            case 3:
                parameters.SetDifficultyParameters(750, 75);
                break;

            default:
                parameters.SetDifficultyParameters(750, 75);
                break;
        }
    }
}
