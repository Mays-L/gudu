using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyMemoryLevelFactory: LevelFactory
{
    public HoneyMemoryParameters parameters = new HoneyMemoryParameters();
    public HoneyMemoryLevelFactory()
    {
        LevelNumber = 34;
    }

    public override void GetParameters()
    {
        switch (CurrentLevel) 
        {
            case 1:
                parameters.SetLevelParameters(1, 4, 1);
                break;
            case 2:
                parameters.SetLevelParameters(2, 6, 1);
                break;
            case 3:
                parameters.SetLevelParameters(3, 8, 1);
                break;
            case 4:
                parameters.SetLevelParameters(3, 9, 1);
                break;
            case 5:
                parameters.SetLevelParameters(4, 12, 2);
                break;
            case 6:
                parameters.SetLevelParameters(5, 12, 1);
                break;
            case 7:
                parameters.SetLevelParameters(5, 15, 2);
                break;
            case 8:
                parameters.SetLevelParameters(6, 16, 1);
                break;
            case 9:
                parameters.SetLevelParameters(4, 16, 3);
                break;
            case 10:
                parameters.SetLevelParameters(5, 16, 4);
                break;
            case 11:
                parameters.SetLevelParameters(7, 18, 2);
                break;
            case 12:
                parameters.SetLevelParameters(8, 20, 1);
                break;
            case 13:
                parameters.SetLevelParameters(8, 20, 2);
                break;
            case 14:
                parameters.SetLevelParameters(6, 20, 3);
                break;
            case 15:
                parameters.SetLevelParameters(6, 20, 4);
                break;
            case 16:
                parameters.SetLevelParameters(9, 24, 1);
                break;
            case 17:
                parameters.SetLevelParameters(10, 25, 2);
                break;
            case 18:
                parameters.SetLevelParameters(10, 28, 1);
                break;
            case 19:
                parameters.SetLevelParameters(11, 30, 2);
                break;
            case 20:
                parameters.SetLevelParameters(11, 35, 1);
                break;
            case 21:
                parameters.SetLevelParameters(12, 36, 2);
                break;
            case 22:
                parameters.SetLevelParameters(13, 40, 2);
                break;
            case 23:
                parameters.SetLevelParameters(13, 40, 3);
                break;
            case 24:
                parameters.SetLevelParameters(14, 42, 2);
                break;
            case 25:
                parameters.SetLevelParameters(15, 42, 2);
                break;
            case 26:
                parameters.SetLevelParameters(16, 42, 2);
                break;
            case 27:
                parameters.SetLevelParameters(14, 48, 3);
                break;
            case 28:
                parameters.SetLevelParameters(15, 48, 3);
                break;
            case 29:
                parameters.SetLevelParameters(16, 49, 3);
                break;
            case 30:
                parameters.SetLevelParameters(14, 49, 4);
                break;
            case 31:
                parameters.SetLevelParameters(15, 56, 4);
                break;
            case 32:
                parameters.SetLevelParameters(16, 56, 4);
                break;
            case 33:
                parameters.SetLevelParameters(17, 64, 3);
                break;
            case 34:
                parameters.SetLevelParameters(17, 64, 4);
                break;

        }
        switch (CurrentDifficulty)
        {
            case 0:
                parameters.SetDifficultyParameters(4f);
                break;

            case 1:
                parameters.SetDifficultyParameters(2f);
                break;

            case 2:
                parameters.SetDifficultyParameters(1f);
                break;

            case 3:
                parameters.SetDifficultyParameters(0.5f);
                break;

            default:
                parameters.SetDifficultyParameters(0.5f);
                break;
        }
    }
}
