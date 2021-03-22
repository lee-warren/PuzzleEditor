using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{

    string filename;

    public GameObject puzzleGameBoard;
    public GameObject palette;
    public GameObject edgePalette;

    public int tileRowColumns = 1;
    public float tileSpacing = 0.1f;
    public Vector2 distanceFromCenterToEdge;
    public Vector2 screenSize;
    public Vector2 bottomLeftCorner;
    
    public GameObject tilePrefab;
    public GameObject[,] tiles;

    public Sprite[] possibleAttributes;
    public GameObject[] paletteTiles;

    public GameObject edgeTilePrefab;
    public Sprite[] possibleEdgeAttributes;
    public GameObject[,] edgeTiles;
    public GameObject[] edgePaletteTiles;

    public Color[] possibleColours;

    public TileAttribute selectedTile;
    public TileAttribute selectedEdgeTile;

    public SaveBoardObject saveBoard;
    public Button button;

    public static GameBoard instance; //A reference to our game control script so we can access it statically.

    void Awake()
    {
        //If we don't currently have a game control...
        if (instance == null)
            //...set this one to be it...
            instance = this;
        //...otherwise...
        else if(instance != this)
            //...destroy this one because it is a duplicate.
            Destroy (gameObject);
    }
    void Start()
    {
        //This will hold all of the information on what should go on the screen
        filename = PlayerPrefs.GetString("levelName");

        loadBoardFromFile();


        //create all of the possible colours:
        possibleColours = new Color[5] {
            new Color(237, 186, 95, 93),
            new Color(247, 101, 99, 97),
            new Color(142, 101, 224, 88),
            new Color(99, 228, 247, 97),
            new Color(154, 240, 137, 94)
            };

        //initialise the pallete, and all of the squares to go in there
        paletteTiles = new GameObject[possibleAttributes.Length];
        for (int j = 0; j < possibleAttributes.Length; j++)
        {
            paletteTiles[j] = (GameObject)Instantiate(tilePrefab, new Vector2(0, 0), Quaternion.identity, palette.transform);
            var tile = paletteTiles[j].GetComponent<Tile>();
            if (tile)
            {
                tile.InitialiseForPalette(j, possibleAttributes[j]);
            }
        }

        //initialise the edgePalette
        //initialise the pallete, and all of the squares to go in there
        edgePaletteTiles = new GameObject[possibleEdgeAttributes.Length];
        for (int j = 0; j < possibleEdgeAttributes.Length; j++)
        {
            edgePaletteTiles[j] = (GameObject)Instantiate(edgeTilePrefab, new Vector2(0, 0), Quaternion.identity, edgePalette.transform);
            var tile = edgePaletteTiles[j].GetComponent<EdgeTile>();
            if (tile)
            {
                tile.InitialiseForPalette(j, possibleEdgeAttributes[j]);
            }
        }

        //initialise all of the empty squares from the tile prefab to go in the game board:
        tiles = new GameObject[tileRowColumns,tileRowColumns];
        for (int i =0; i< tileRowColumns; i++)
        {
            for (int j =0; j< tileRowColumns; j++)
            {
                tiles[i,j] = (GameObject)Instantiate(tilePrefab, new Vector2(0,0), Quaternion.identity, puzzleGameBoard.transform);
                var tile = tiles[i,j].GetComponent<Tile>();
                if(tile)
                {
                    tile.InitialiseForPuzzleGameBoard(new Vector2(i,j));
                }
            } 
        }
        

        //create all the empty squares for the edge tiles:
        edgeTiles = new GameObject[4, tileRowColumns];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < tileRowColumns; j++)
            {
                edgeTiles[i, j] = (GameObject)Instantiate(edgeTilePrefab, new Vector2(0, 0), Quaternion.identity, puzzleGameBoard.transform);
                var tile = edgeTiles[i, j].GetComponent<EdgeTile>();
                if (tile)
                {
                    tile.InitialiseForPuzzleGameBoard(i, j);
                }
            }
        }

        //fill the board based on the level that was loaded:
        if (saveBoard.boardLength != 0)
        {
            loadBoardTilesOntoBoard();
        }


        button.GetComponent<Button>().onClick.AddListener(saveBoardToFile);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void saveBoardToFile()
    {

        saveBoard = new SaveBoardObject(tileRowColumns);

        //saves the edge tiles:
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < tileRowColumns; j++)
            {
                var currentTile = edgeTiles[i, j].GetComponent<EdgeTile>();
                if (currentTile.edgeTileAttribute)
                {
                    var currentAttribute = currentTile.edgeTileAttribute.GetComponent<TileAttribute>();
                    if (currentAttribute)
                    {
                        Debug.Log(currentAttribute.name);
                        var saveEdgeTile = new SaveEdgeTile();
                        /*
                        public int side; //side is which of the strips it will fall into (top, left, right, bottom)
                        public int index; //index is which index of that row it will fall into

                        public string type;
                        public int colorArrayNumber = -1; //no colour
                        */
                        saveEdgeTile.side = currentTile.side;
                        saveEdgeTile.index = currentTile.index;

                        saveEdgeTile.type = currentAttribute.type;
                        saveEdgeTile.colorArrayNumber = currentAttribute.colorArrayNumber;

                        saveBoard.edgeTiles.Add(saveEdgeTile);
                    }
                }
            }
        }

        //saves the game tiles:
        for (int i = 0; i < tileRowColumns; i++)
        {
            for (int j = 0; j < tileRowColumns; j++)
            {
                var currentTile = tiles[i, j].GetComponent<Tile>();
                if (currentTile.tileAttribute)
                {
                    var currentAttribute = currentTile.tileAttribute.GetComponent<TileAttribute>();
                    if (currentAttribute)
                    {
                        Debug.Log(currentAttribute.name);
                        var saveGameTile = new SaveGameTile();
                        /*
                        positionX;
                        public int positionY;

                        public string type;
                        public int rotation = 0;
                        public int colorArrayNumber = -1; //no colour
                        public bool isLocked
                        */
                        saveGameTile.positionX = (int)currentTile.position.x;
                        saveGameTile.positionY = (int)currentTile.position.y;

                        saveGameTile.type = currentAttribute.type;
                        saveGameTile.colorArrayNumber = currentAttribute.colorArrayNumber;
                        saveGameTile.rotation = currentAttribute.rotation;
                        saveGameTile.isLocked = false;

                        saveBoard.gameTiles.Add(saveGameTile);
                    }
                }
            }
        }

        // Create an instance of the XmlSerializer class;
        // specify the type of object to serialize.
        XmlSerializer serializer = new XmlSerializer(typeof(SaveBoardObject));
        TextWriter writer = new StreamWriter(filename);

        // Serialize the purchase order, and close the TextWriter.
        serializer.Serialize(writer, saveBoard);
        writer.Close();

        ReturnToMenu();
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("Scenes/LevelSelect");
    }

    public void loadBoardFromFile()
    {
        //test read:
        if (File.Exists(filename))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SaveBoardObject));
            FileStream fs = new FileStream(filename, FileMode.Open);

            saveBoard = (SaveBoardObject)serializer.Deserialize(fs);
            Debug.Log("Game data loaded!");

            tileRowColumns = saveBoard.boardLength;
        }
        else { 
            Debug.LogError("There is no save data!");
            //loadBoardFresh
        }
    }


    public void loadBoardTilesOntoBoard()
    {
        //load edge tiles in
        for (int i = 0; i < saveBoard.edgeTiles.Count; i++)
        {
            var edgeTileToAdd = saveBoard.edgeTiles[i];

            edgeTiles[edgeTileToAdd.side, edgeTileToAdd.index].GetComponent<EdgeTile>().edgeTileAttribute = (GameObject)Instantiate(edgeTiles[edgeTileToAdd.side, edgeTileToAdd.index].GetComponent<EdgeTile>().edgeTileAttributePrefab, new Vector2(0, 0), Quaternion.identity, edgeTiles[edgeTileToAdd.side, edgeTileToAdd.index].GetComponent<EdgeTile>().transform);
            var attributeTile = edgeTiles[edgeTileToAdd.side, edgeTileToAdd.index].GetComponent<EdgeTile>().edgeTileAttribute.GetComponent<TileAttribute>();
            if (attributeTile)
            {
                attributeTile.CopyFromEdgeSave(edgeTileToAdd);
                attributeTile.RotateToFaceCenter(edgeTiles[edgeTileToAdd.side, edgeTileToAdd.index].GetComponent<EdgeTile>().position);
            }

            edgeTiles[edgeTileToAdd.side, edgeTileToAdd.index].GetComponent<EdgeTile>().spriteRenderer.sprite = edgeTiles[edgeTileToAdd.side, edgeTileToAdd.index].GetComponent<EdgeTile>().mySecondSprite;
        }

        //load game tiles in
        for (int i = 0; i < saveBoard.gameTiles.Count; i++)
        {
            var gameTileToAdd = saveBoard.gameTiles[i];

            tiles[gameTileToAdd.positionX, gameTileToAdd.positionY].GetComponent<Tile>().tileAttribute = (GameObject)Instantiate(tiles[gameTileToAdd.positionX, gameTileToAdd.positionY].GetComponent<Tile>().tileAttributePrefab, new Vector2(0, 0), Quaternion.identity, tiles[gameTileToAdd.positionX, gameTileToAdd.positionY].GetComponent<Tile>().transform);
            var attributeTile = tiles[gameTileToAdd.positionX, gameTileToAdd.positionY].GetComponent<Tile>().tileAttribute.GetComponent<TileAttribute>();
            if (attributeTile)
            {
                attributeTile.CopyFromGameSave(gameTileToAdd);
            }

            tiles[gameTileToAdd.positionX, gameTileToAdd.positionY].GetComponent<Tile>().spriteRenderer.sprite = tiles[gameTileToAdd.positionX, gameTileToAdd.positionY].GetComponent<Tile>().mySecondSprite;
        }

    }

}