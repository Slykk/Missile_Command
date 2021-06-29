using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefenderData
{
    public int ammunition;
    public int health;
    public Vector2 position;
    public GameObject defender;
}
[System.Serializable]
public class BuildingData
{
    public int health;
    public Vector2 position;
    public GameObject building;
}
[System.Serializable]
public class MissileData
{
    public int damage;
    public float speed;
    public Vector2 position;
    public Vector2 spawn;
    public Vector2 target;
}


public class GameLogic : MonoBehaviour
{
    public List<DefenderData> defenderList = new List<DefenderData>();
    public List<BuildingData> buildingList = new List<BuildingData>();
    public MissileData missile = new MissileData();


    // Start is called before the first frame update
    void Start()
    {
        
        
        missile.speed = 5f;
        missile.spawn = new Vector2(2,2);
        Instantiate((GameObject)Resources.Load("/Prefabs/Missile", typeof(GameObject)), missile.spawn, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
