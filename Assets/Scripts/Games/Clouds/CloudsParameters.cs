using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represent parameters for each round of game
/// </summary>
public class CloudsParameters : LevelParameters
{

    /// <summary>
    ///Number of clouds in the game between 12 to 20
    /// </summary>
    public int CloudsNumber { get; internal set; } = 12;

    /// <summary>
    /// Number of rainy clouds between 1 to 10
    /// </summary>
    public int RainyCloudsNumber { get; internal set; } = 3;


    /// <summary>
    /// Number of areas which include atleast a cloud
    /// </summary>
    public int NumberOfRainyAreas { get; internal set; } = 1;

    public int InitialSpeed { get; internal set; } = 0;
    public int Acceleration { get; internal set; } = 0;


    public void SetLevelParameters(int cloudsNumbere, int rainyCloudsNumber, int numberOfRainyAreas)
    {
        CloudsNumber = cloudsNumbere;
        RainyCloudsNumber = rainyCloudsNumber;
        NumberOfRainyAreas = numberOfRainyAreas;
    }

    public void SetDifficultyParameters(int _initialSpeed, int _acceleration)
    {
        InitialSpeed = _initialSpeed;
        Acceleration = _acceleration;
    }

}
