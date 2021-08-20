using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{    
    private int levelIndexToLoad;
    private Transform menuParent;
    private Transform levelParent;
    private Button playButton;
    private List<Button> levelList = new List<Button>();
    private Transform crossFade;

    public Animator transition;
    public float transitionTime = 1f;

    void Awake()
    {
        
    }

    void Start()
    {
        PauseMenu.gameIsPaused = false;

        menuParent = transform.Find("Menu");
        levelParent = transform.Find("Canvas/Main Menu/Levels/Levels List");
        playButton = transform.Find("Canvas/Main Menu/Menu/Play").GetComponent<Button>();
        crossFade = transform.Find("Canvas/Crossfade");

        // JSON Data initialization - singleton
        GameData.GetInstance().Init();

        if (!PlayerPrefs.HasKey("lastLevelPlayed"))
        {
            levelIndexToLoad = 0;
            PlayerPrefs.SetInt("lastLevelPlayed", levelIndexToLoad);
            playButton.GetComponentInChildren<Text>().text = "New Game";
            playButton.onClick.AddListener(delegate {LoadLevel(levelIndexToLoad); });
        }
        else if (PlayerPrefs.GetInt("lastLevelPlayed") == 0)
        {
            levelIndexToLoad = 0;
            playButton.GetComponentInChildren<Text>().text = "New Game";
            playButton.onClick.AddListener(delegate {LoadLevel(levelIndexToLoad); });
        }
        else
        {
            levelIndexToLoad =  PlayerPrefs.GetInt("lastLevelPlayed");
            playButton.GetComponentInChildren<Text>().text = "Continue";
            playButton.onClick.AddListener(delegate {LoadLevel(levelIndexToLoad); });
        }


        // Create Level List
        for (int i=0; i < GameData.GetInstance().LevelsCount(); i++)
        {
            int levelIndex = i;
            levelList.Add(Instantiate((Button)Resources.Load("Prefabs/Button", typeof (Button)), Vector3.zero, Quaternion.identity));
            levelList[i].transform.SetParent(levelParent, false);
            levelList[i].transform.GetComponentInChildren<Text>().text = "Level " + (1 + i).ToString();
            levelList[i].onClick.AddListener(delegate {LoadLevel(levelIndex); });
        }
    }

    

    public void LoadLevel(int levelIndex)
    {
        GameData.GetInstance().SetCurrentLevel(levelIndex);
        crossFade.gameObject.SetActive(true);
        // SceneManager.LoadScene("Game");
        StartCoroutine(LoadLevelTransition());
    }

    IEnumerator LoadLevelTransition()
    {
        // Play Animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load Scene
        SceneManager.LoadScene("Game");
    }

    public void LevelSelection()
    {
        // hide menu
        GameObject.Find("Canvas/Main Menu/Menu").SetActive(false);
        GameObject.Find("Canvas/Main Menu/Levels").SetActive(true);
    }

    public void Return()
    {
        GameObject.Find("Canvas/Main Menu/Menu").SetActive(true);
        GameObject.Find("Canvas/Main Menu/Levels").SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        
    }
}
