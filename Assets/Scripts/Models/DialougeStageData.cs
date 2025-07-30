using System;
using System.Collections.Generic;
using UnityEngine;

// Represents one stage's dialog text and animal sprite name
[Serializable]
public class DialogueStageData
{
    public int    stageIndex;
    public string message;
    public string imageName;
}

// Wrapper to match JSON structure: { "stages": [ ... ] }
[Serializable]
public class DialogueStageDataList
{
    public List<DialogueStageData> stages;
}
