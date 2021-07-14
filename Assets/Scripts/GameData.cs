using UnityEngine;


public class GameData
{
    private static GameData instance;
    public static GameData GetInstance()
    {
        if (instance == null)
            instance = new GameData();
        return instance;
    }


    private JsonData jsonData = new JsonData();
    private LevelData currLevel = new LevelData();
    private DifficultyData currDifficulty = new DifficultyData();


    public void Init()
    {
        // Json Data handling
        TextAsset asset = (TextAsset)Resources.Load("Data/GameData");
        string jsonText = asset.text;
        jsonData = JsonUtility.FromJson<JsonData>(jsonText);
    }

    public LevelData GetLevel(int levelIndex)
    {
        return jsonData.levels[levelIndex];
    }

    public DifficultyData GetDifficulty(int difficultyInex)
    {
        return jsonData.difficulty[difficultyInex];
    }
    

}


[System.Serializable]
public class LevelData
{
    public string layoutName;
    public float waveTimer;
    public int missilesAmount;
}
[System.Serializable]
public class DifficultyData
{
    public string difficulty;
    public float missileSpeedModifier;
    public float waveFrequencyModifier;
}
[System.Serializable]
public class JsonData
{
    public LevelData[] levels;
    public DifficultyData[] difficulty;
}