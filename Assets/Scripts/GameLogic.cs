using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class GameLogic : MonoBehaviour
{
    private List<Defender> defenderList;
    private List<Building> buildingList;
    public List<GameObject> missileList;

    public GameObject missilesParent;
    public GameObject defenderMissilesParent;
    private GameObject missile;
    private GameObject defenderMissile;
    public float timer;
    private List<string> targetType;
    private Vector3 missileTarget;
    private int missilesLeft;
    private int waveMissiles;

    void Awake()
    {
        
    }

    void Start()
    {
        CreateLevel();
        Debug.LogWarning(GameData.GetInstance().currLevel.missilesAmount);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
            FireDefenderMissile(mousePosition);
        }
        
        if (missilesLeft > 0)
        {
            if (timer <= 0)
            {
                timer = GameData.GetInstance().currLevel.waveTimer;
                int random = Random.Range(1, Mathf.FloorToInt(missilesLeft/3f));
                waveMissiles = random;
                missilesLeft -= random;
                for (int i = 0; i < waveMissiles; i++)
                {
                    CreateMissile();
                }
            }
        }
        else if (missileList.Count == 0)
        {
            // Load next level
            GameData.GetInstance().SetNextLevel();
            CreateLevel();
            // Debug.LogWarning("You have completed the level!");
            // Debug.LogWarning("list is " + missileList.Count);
        }
    }

    private void CreateLevel()
    {
        var layout = GameObject.FindGameObjectWithTag("Layout");
        if (layout != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Layout"));
        }
        // Getting data from GameData and setting layout
        //currLevel = GameData.GetInstance().GetCurrentLevel();

        timer = GameData.GetInstance().currLevel.waveTimer;
        missilesLeft = GameData.GetInstance().currLevel.missilesAmount;

        var currLayout =  Instantiate((GameObject)Resources.Load("Prefabs/" + GameData.GetInstance().currLevel.layoutName, typeof(GameObject)), Vector3.zero, Quaternion.identity);
        currLayout.name = GameData.GetInstance().currLevel.layoutName;
        currLayout.transform.SetParent(this.transform);

        // Lists by the components in children of the instantiated layout parent
        buildingList = new List<Building>(transform.Find(GameData.GetInstance().currLevel.layoutName).GetComponentsInChildren<Building>());
        defenderList = new List<Defender>(transform.Find(GameData.GetInstance().currLevel.layoutName).GetComponentsInChildren<Defender>());
        missileList = new List<GameObject>();
    }
    

    private void CreateMissile()
    {
        // def : 3, bld : 2 - to be changed late with other balancing set by difficulty or level, etc
        int defChance = 3;
        int buildingChance = 2;

        List<Vector3> targets = new List<Vector3>();
        foreach (var item in buildingList)
        {
            if (!item.IsAlive())
                continue;
            for (int i = 0; i < buildingChance; i++)
                targets.Add(item.GetMissileTarget());
        }

        foreach (var item in defenderList)
        {
            if (!item.IsAlive())
                continue;
            for (int i = 0; i < defChance; i++)
                targets.Add(item.GetMissileTarget());
        }

        Vector3 target = targets[Random.Range(0, targets.Count)];

        // Missile creation
        missile = Instantiate((GameObject)Resources.Load("Prefabs/Missile", typeof(GameObject)), new Vector3(Random.Range(-9,9), 11f, 0f), Quaternion.identity);
        missile.transform.SetParent(missilesParent.transform);
        missile.GetComponent<Missile>().Init(2f, target, 2);
        missileList.Add(missile);
    }

    private void FireDefenderMissile(Vector3 target)
    {
        // example for instantiating a text prefab
        // var canvas = transform.Find("FloatingText");
        // var number = Instantiate((GameObject)Resources.Load("Prefabs/Number"), canvas).GetComponent<ScorePopup>();
        // number.Init("8", target);
        
        // return;

        // Find closest available defender - not destroyed + not loading + has ammo
        List<Defender> avialableDefenders = new List<Defender>();


        foreach (var defender in defenderList)
        {
            if (defender.operational)
            {
                avialableDefenders.Add(defender);
            }
        }

        Defender closestDefender = null;
        float minDist = Mathf.Infinity;
        foreach (var defender in avialableDefenders)
        {
            float dist = Vector3.Distance(defender.transform.position, target);
            if (dist < minDist)
            {
                closestDefender = defender;
                minDist = dist;
            }
        }
        if (closestDefender == null)
            return;
        closestDefender.ammo--;
        closestDefender.reloading = true;
        Vector3 spawnPosition = closestDefender.transform.position;
        defenderMissile = Instantiate((GameObject)Resources.Load("Prefabs/DefenderMissile", typeof(GameObject)), spawnPosition, Quaternion.identity);
        defenderMissile.transform.SetParent(defenderMissilesParent.transform);
        defenderMissile.GetComponent<DefenderMissile>().Init(4f, target, 1f, 2f);
    }
}
