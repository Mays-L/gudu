[System.Serializable]
public struct PlayerLevelData
{
    private int levelNumber;
    public int LevelNumber
    {
        get
        {
            return LevelNumber;
        }
        set
        {
            if (value > 200) levelNumber = 200;
            else if (value < 1) levelNumber = 1;
            else levelNumber = value;
        }
    }
    public int TrueAnswerNumber { set; get; }
    public int FalseAnswerNumber { set; get; }
    public float TimeToPass { set; get; }

}
