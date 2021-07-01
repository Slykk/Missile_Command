using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
public class DifficultyData
{
    public string difficulty;
    public float missileSpeedModifier;
    public float waveFrequency;
}
[System.Serializable]
public class JsonData
{
    public LevelData[] levels;
    public DifficultyData[] difficulties;
}


public class GameLogic : MonoBehaviour
{
    private List<Defender> defenderList;
    private List<Building> buildingList;
    // private List<Missile> missileList;

    public GameObject missilesParent;
    private GameObject missile;
    private LevelData currLevel;
    public float timer;
    private int random;
    private List<string> targetType;
    private Vector3 missileTarget;
    private int missilesLeft;
    private int waveMissiles;

    void Awake()
    {
        buildingList = new List<Building>(transform.GetComponentsInChildren<Building>());
        defenderList = new List<Defender>(transform.GetComponentsInChildren<Defender>());
        
        targetType = new List<string>();
        targetType.Add("Building");
        targetType.Add("Defender");

        currLevel = new LevelData();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //Default value - to be changed later in JSON
        currLevel.waveTimer = 2f;
        currLevel.missilesAmount = 1;

        Debug.LogWarning(currLevel.waveTimer);
        timer = currLevel.waveTimer;
        missilesLeft = currLevel.missilesAmount;

        //Debug.LogWarning(timer);
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        //Debug.LogWarning(timer);
        if (timer <= 0)
        {
            timer = currLevel.waveTimer;

            random = Random.Range(1, Mathf.FloorToInt(missilesLeft/3f)+1);
            waveMissiles = random;
            missilesLeft -= random;
            for (int i = 0; i < waveMissiles; i++)
            {
                CreateMissile();
            }
        }
        if (missilesLeft == 0)
        {
            Debug.LogWarning("You have completed the level!");
        }
    }

    void CreateMissile()
    {
        // Assign target randomly
        random = Random.Range(0, targetType.Count);
        Debug.LogWarning("There are: " + missilesLeft);
        if (targetType[random] == "Building")
        {
            random = Random.Range(0, buildingList.Count);
            missileTarget = buildingList[random].GetMissileTarget();
        }
        else if (targetType[random] == "Defender")
        {
            random = Random.Range(0, defenderList.Count);
            missileTarget = defenderList[random].GetMissileTarget();
        }

        // Missile creation
        missile = Instantiate((GameObject)Resources.Load("Prefabs/Missile", typeof(GameObject)), new Vector3(Random.Range(-9,9), 11f, 0f), Quaternion.identity);
        missile.transform.SetParent(missilesParent.transform);
        missile.GetComponent<Missile>().Init(3f, missileTarget, 20);
    }
}
