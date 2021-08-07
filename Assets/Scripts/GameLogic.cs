using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
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
    private Transform currLayout;
    private GameObject levelCompleteWindow;
    
    Sequence mySequence = DOTween.Sequence();
    

    void Awake()
    {
        // Temp JSON DATA for testing purposes
        GameData.GetInstance().Init();
        GameData.GetInstance().SetCurrentLevel(0);
    }

    void Start()
    {
        levelCompleteWindow = transform.Find("Canvas/LevelCompleteWindow").gameObject;
        levelCompleteWindow.SetActive(false);

        CreateLevel();
        Debug.LogWarning(GameData.GetInstance().currLevel.missilesAmount);

    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // Return after testing
                
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
                //FireDefenderMissile(mousePosition);
                

                //testing purposes code
                //SHOOT IRON DOME STYLE BUNCHA ANTI MISSILE MISSILES
                for(int i = 0; i < missileList.Count; i++)
                {
                    GameObject missile = missileList[i];
                    if (missile.transform.position.y > 3.5f)
                    {
                        Transform tempMissile = Instantiate((GameObject)Resources.Load("Prefabs/IronDomeMissile", typeof(GameObject)), Vector3.zero, Quaternion.identity).transform;
                        Vector3 hitPos = missile.transform.position + missile.GetComponent<Missile>().direction.normalized * 3;
                        tempMissile.gameObject.GetComponent<IronDomeMissile>().Init(hitPos);

                        //mySequence.Insert(2f * i, tempMissile.DOMove(hitPos, 2f));
                        
                    }
                }

            }
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
            levelCompleteWindow.SetActive(true);
            
            //GameData.GetInstance().SetNextLevel();
            //CreateLevel();
        }

        foreach (Building building in buildingList)
        {
            if (building.IsAlive())
                break;
            
            // Game Over
        }
    }

    public void LoadNextLevel()
    {
        GameData.GetInstance().SetNextLevel();
        CreateLevel();
        levelCompleteWindow.SetActive(false);
    }

    public void QuitLevel()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void CreateLevel()
    {
        if (currLayout != null)
        {
            Destroy(currLayout.gameObject);
        }
        // Getting data from GameData and setting layout
        //currLevel = GameData.GetInstance().GetCurrentLevel();
        
        timer = GameData.GetInstance().currLevel.waveTimer;
        missilesLeft = GameData.GetInstance().currLevel.missilesAmount;

        currLayout = Instantiate(Resources.Load("Prefabs/" + GameData.GetInstance().currLevel.layoutName) as GameObject, this.transform).transform;
        currLayout.name = GameData.GetInstance().currLevel.layoutName;

        // Lists by the components in children of the instantiated layout parent
        buildingList = new List<Building>(currLayout.GetComponentsInChildren<Building>());
        defenderList = new List<Defender>(currLayout.GetComponentsInChildren<Defender>());
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