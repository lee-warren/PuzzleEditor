using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject worldTilePrefab;

    public GameObject worldSelectScreen;
    public GameObject worldSelectBoard;
    public GameObject[] worldTiles;

    public string[] worldNames;

    public GameObject levelSelectScreen;
    public GameObject levelSelectBoard;
    public Button returnToWorldsButton;
    public GameObject[] levelTiles;

    string[] levelNames;

    public static LevelSelector instance; //A reference to our game control script so we can access it statically.

    void Awake()
    {
        //If we don't currently have a game control...
        if (instance == null)
            //...set this one to be it...
            instance = this;
        //...otherwise...
        else if (instance != this)
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadWorldScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadWorldScreen()
    {
        levelSelectScreen.SetActive(false);
        worldSelectScreen.SetActive(true);

        worldNames = Directory.GetDirectories(Application.persistentDataPath + "/Levels/");

        //initialise the world tiles
        worldTiles = new GameObject[worldNames.Length + 1];
        for (int j = 0; j < worldNames.Length; j++)
        {
            worldTiles[j] = (GameObject)Instantiate(worldTilePrefab, new Vector2(0, 0), Quaternion.identity, worldSelectBoard.transform);
            var tile = worldTiles[j].GetComponent<WorldSelectTile>();
            if (tile)
            {
                tile.InitialiseForWorlds(j, worldNames[j]);
            }
        }
        //create 'add new level' button
        if (worldNames.Length < 15)
        {
            worldTiles[worldNames.Length] = (GameObject)Instantiate(worldTilePrefab, new Vector2(0, 0), Quaternion.identity, worldSelectBoard.transform);
            var tile = worldTiles[worldNames.Length].GetComponent<WorldSelectTile>();
            if (tile)
            {
                tile.InitialiseForAddWorldButton(worldNames.Length, Application.persistentDataPath + "/Levels/");
            }
        }
    }

    public void LoadLevelScreen(string worldName)
    {
        //Note: worldName is taking the whole file path for the world at the moment, not just the world name specifically

        worldSelectScreen.SetActive(false);
        levelSelectScreen.SetActive(true);

        levelNames = Directory.GetFiles(worldName);

        returnToWorldsButton.GetComponent<Button>().onClick.AddListener(ReturnToWorldSelect);

        //initialise the level tiles
        levelTiles = new GameObject[levelNames.Length + 1];
        for (int j = 0; j < levelNames.Length; j++)
        {
            levelTiles[j] = (GameObject)Instantiate(tilePrefab, new Vector2(0, 0), Quaternion.identity, levelSelectBoard.transform);
            var tile = levelTiles[j].GetComponent<LevelSelectTile>();
            if (tile)
            {
                tile.InitialiseForLevels(j, levelNames[j]);
            }
        }
        //create 'add new level' button
        if (levelNames.Length < 15)
        {
            levelTiles[levelNames.Length] = (GameObject)Instantiate(tilePrefab, new Vector2(0, 0), Quaternion.identity, levelSelectBoard.transform);
            var tile = levelTiles[levelNames.Length].GetComponent<LevelSelectTile>();
            if (tile)
            {
                tile.InitialiseForAddLevelButton(levelNames.Length, worldName);
            }
        }
    }

    public void ReturnToWorldSelect()
    {
        //Load world screen stuff
        LoadWorldScreen();
    }

    public void GoToLevel(string levelName)
    {
        PlayerPrefs.SetString("levelName", levelName); //PlayerPrefs.SetString("levelName", Application.persistentDataPath + "/" + levelName);

        SceneManager.LoadScene("Scenes/GameBoard");
    }
}
