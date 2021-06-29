using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefenderData
{
    public int ammunition;
    public int health;
}
[System.Serializable]
public class BuildingData
{
    public int health;
}
[System.Serializable]
public class MissileData
{
    public int damage;
    public float speed;
}
[System.Serializable]
public class LevelData
{
    public float waveTimer;
    public int missilesAmount;
}
[System.Serializable]
public class difficultyData
{
    public string difficulty;
    public float missileSpeedModifier;
    public float waveFrequency;
}
[System.Serializable]
public class JsonData
{
    public LevelData[] levels;
    public difficultyData[] difficulties;
}


public class GameLogic : MonoBehaviour
{
    private List<Defender> defenderList;
    private List<Building> buildingList;
    // private List<Missile> missileList;

    GameObject missile = new GameObject();
    LevelData currLevel = new LevelData();
    public float timer;

    void Awake()
    {
        buildingList = new List<Building>(transform.GetComponentsInChildren<Building>());
        defenderList = new List<Defender>(transform.GetComponentsInChildren<Defender>());
    }

    // Start is called before the first frame update
    void Start()
    {
        
        //LevelData currLevel = new LevelData();
        //Default value - to be changed later in JSON
        currLevel.waveTimer = 5f;
        Debug.LogWarning(currLevel.waveTimer);
        timer = currLevel.waveTimer;
        //Debug.LogWarning(timer);


        missile = Instantiate((GameObject)Resources.Load("Prefabs/Missile", typeof(GameObject)), new Vector3(Random.Range(-9,9), 11f, 0f), Quaternion.identity);
        missile.GetComponent<Missile>().Init(3f, Vector3.zero, 20);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        //Debug.LogWarning(timer);
        if (timer <= 0)
        {
            timer = currLevel.waveTimer;
            missile = Instantiate((GameObject)Resources.Load("Prefabs/Missile", typeof(GameObject)), new Vector3(Random.Range(-9,9), 11f, 0f), Quaternion.identity);
            missile.GetComponent<Missile>().Init(3f, Vector3.zero, 20);
        }
    }
}
