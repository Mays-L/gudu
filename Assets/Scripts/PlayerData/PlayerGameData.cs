using System.Collections.Generic;

[System.Serializable]
public struct PlayerGameData
{
    public List<PlayerLevelData> PlayerLevelDataList { get; set; }
    public int Score { get; set; }
}
