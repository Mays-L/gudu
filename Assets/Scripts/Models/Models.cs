using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Possible directions at each crossroad
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

// Records one decision at a 4-way fork
public class CrossroadChoice
{
    public int crossroadIndex;      // which fork (1–4)
    public Vector2 entryPosition;      // player position when dialog appeared
    public Direction chosenDirection;   // direction the player chose
    public bool isCorrect;          // was it the right path?
    public float responseTime;       // time between dialog show and direction chosen

    internal float responseStartTime;   // internal: timestamp when dialog shown
}

// Aggregates all choices and timing for one stage (one house run)
public class StageData
{
    public int stageIndex;         // stage number (1–4)
    public float stageStartTime;     // Time.time when stage began
    public float stageEndTime;       // Time.time when stage ended
    public List<CrossroadChoice> choices = new List<CrossroadChoice>();

    public int WrongCount => choices.Count(c => !c.isCorrect);

    public List<Direction> WrongDirections =>
        choices.Where(c => !c.isCorrect)
               .Select(c => c.chosenDirection)
               .ToList();

    public float TotalStageTime => stageEndTime - stageStartTime;
}

// Persists across scenes and holds all stage data
public class GameSession
{
    public List<StageData> allStages = new List<StageData>();

    public int TotalCorrectPaths =>
        allStages.Sum(stage => stage.choices.Count(c => c.isCorrect));

    public int TotalWrongPaths =>
        allStages.Sum(stage => stage.choices.Count(c => !c.isCorrect));

    public int TotalHousePoints => allStages.Count;
}
