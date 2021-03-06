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
    //general components
    string filename;
    public SaveBoardObject saveBoard;
    public int tileRows;
    public int tileColumns;

    public float tileSpacing = 0.1f;

    public InputField enteredHeigthInput;
    public InputField enteredWidthInput;

    public List<LazorColour> primaryColours;
    public List<LazorColour> secondaryColours;

    //

    //Config Page stuff
    public Canvas levelConifgOptionsCanvas;

    public Button saveConfigButton;
    //

    //Game Board stuff
    public Canvas gameBoardCanvas;

    public GameObject puzzleGameBoard;
    public GameObject palette;
    public GameObject edgePalette;
    public GameObject editOptionsPalette;

    public Vector2 distanceFromCenterToEdge;
    public Vector2 screenSize;
    public Vector2 bottomLeftCorner;
    
    public GameObject tilePrefab;
    public GameObject[,] tiles;
    public Sprite[] possibleAttributes;
    public GameObject[] paletteTiles;

    public GameObject edgeTilePrefab;
    public Sprite[] possibleEdgeAttributes;
    public GameObject[][] edgeTiles;
    public GameObject[] edgePaletteTiles;

    public GameObject editOptionsTilePrefab;
    public Sprite[] possibleEditOptionsAttributes;
    public GameObject[] editOptionsPaletteTiles;

    public TileAttribute selectedTile;
    public TileAttribute selectedEdgeTile;
    public TileAttribute selectedEditOptionsTile;

    public Button exportLevelButton;
    public Button changeConfigButton;
    //

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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadEmptyBoard()
    {
        levelConifgOptionsCanvas.gameObject.SetActive(false);
        gameBoardCanvas.gameObject.SetActive(true);

        //create all of the possible colours:
        primaryColours = new List<LazorColour>();
        primaryColours.Add(new LazorColour("red", new Color(255.0f / 255.0f, 5.0f / 255.0f, 5.0f / 255.0f)));
        primaryColours.Add(new LazorColour("yellow", new Color(252, 236, 0)));
        primaryColours.Add(new LazorColour("blue", new Color(0, 0, 255)));

        secondaryColours = new List<LazorColour>();
        secondaryColours.Add(new LazorColour("orange", new Color(255.0f / 255.0f, 126.0f / 255.0f, 0.0f), primaryColours.Find(t => t.colourName == "red"), primaryColours.Find(t => t.colourName == "yellow")));
        secondaryColours.Add(new LazorColour("purple", new Color(186, 0, 241), primaryColours.Find(t => t.colourName == "red"), primaryColours.Find(t => t.colourName == "blue")));
        secondaryColours.Add(new LazorColour("green", new Color(0, 255, 0), primaryColours.Find(t => t.colourName == "blue"), primaryColours.Find(t => t.colourName == "yellow")));


        //destroy any existing children of the palette, edge palette and game board
        foreach (Transform child in palette.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in edgePalette.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in editOptionsPalette.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in puzzleGameBoard.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

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

        //initialise the editOptionsPalette editOptions
        //initialise the pallete, and all of the squares to go in there
        editOptionsPaletteTiles = new GameObject[possibleEditOptionsAttributes.Length];
        for (int j = 0; j < possibleEditOptionsAttributes.Length; j++)
        {
            editOptionsPaletteTiles[j] = (GameObject)Instantiate(editOptionsTilePrefab, new Vector2(0, 0), Quaternion.identity, editOptionsPalette.transform);
            var tile = editOptionsPaletteTiles[j].GetComponent<EditOptionsTile>();
            if (tile)
            {
                tile.InitialiseForPalette(j, possibleEditOptionsAttributes[j]);
            }
        }

        //initialise all of the empty squares from the tile prefab to go in the game board:
        tiles = new GameObject[tileColumns, tileRows];
        for (int i = 0; i < tileColumns; i++)
        {
            for (int j = 0; j < tileRows; j++)
            {
                tiles[i, j] = (GameObject)Instantiate(tilePrefab, new Vector2(0, 0), Quaternion.identity, puzzleGameBoard.transform);
                var tile = tiles[i, j].GetComponent<Tile>();
                if (tile)
                {
                    tile.InitialiseForPuzzleGameBoard(new Vector2(i, j));
                }
            }
        }


        //create all the empty squares for the edge tiles:
        edgeTiles = new GameObject[4][];
        edgeTiles[0] = new GameObject[tileColumns]; //top
        edgeTiles[1] = new GameObject[tileRows]; //left
        edgeTiles[2] = new GameObject[tileRows]; //right
        edgeTiles[3] = new GameObject[tileColumns]; //bottom

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < edgeTiles[i].Length; j++)
            {
                edgeTiles[i][j] = (GameObject)Instantiate(edgeTilePrefab, new Vector2(0, 0), Quaternion.identity, puzzleGameBoard.transform);
                var tile = edgeTiles[i][j].GetComponent<EdgeTile>();
                if (tile)
                {
                    tile.InitialiseForPuzzleGameBoard(i, j);
                }
            }
        }

        exportLevelButton.GetComponent<Button>().onClick.AddListener(saveBoardToFile);
        changeConfigButton.GetComponent<Button>().onClick.AddListener(ShowConfigOptions);
    }

    public void saveBoardToFile()
    {

        saveBoard = new SaveBoardObject(tileColumns, tileRows);

        //saves the edge tiles:
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < edgeTiles[i].Length; j++)
            {
                var currentTile = edgeTiles[i][j].GetComponent<EdgeTile>();
                if (currentTile.edgeTileAttribute)
                {
                    var currentAttribute = currentTile.edgeTileAttribute.GetComponent<TileAttribute>();
                    if (currentAttribute)
                    {
                        Debug.Log(currentAttribute.name);
                        var saveEdgeTile = new SaveEdgeTile();
                        
                        saveEdgeTile.side = currentTile.side;
                        saveEdgeTile.index = currentTile.index;

                        saveEdgeTile.type = currentAttribute.type;
                        saveEdgeTile.colorArrayNumber = currentAttribute.colorArrayNumber;
                        saveEdgeTile.colourName = currentAttribute.mainTileColour.colourName;
                        //saveEdgeTile.isLocked = currentAttribute.isLocked;

                        saveBoard.edgeTiles.Add(saveEdgeTile);
                    }
                }
            }
        }

        //saves the game tiles:
        for (int i = 0; i < tileColumns; i++)
        {
            for (int j = 0; j < tileRows; j++)
            {
                var currentTile = tiles[i, j].GetComponent<Tile>();
                if (currentTile.tileAttribute)
                {
                    var currentAttribute = currentTile.tileAttribute.GetComponent<TileAttribute>();
                    if (currentAttribute)
                    {
                        Debug.Log(currentAttribute.name);
                        var saveGameTile = new SaveGameTile();
                       
                        saveGameTile.positionX = (int)currentTile.position.x;
                        saveGameTile.positionY = (int)currentTile.position.y;

                        saveGameTile.type = currentAttribute.type;
                        saveGameTile.colorArrayNumber = currentAttribute.colorArrayNumber;
                        saveGameTile.colourName = currentAttribute.mainTileColour.colourName;

                        saveGameTile.rotation = currentAttribute.rotation;
                        saveGameTile.isLocked = currentAttribute.isLocked;

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
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SaveBoardObject));
                FileStream fs = new FileStream(filename, FileMode.Open);

                saveBoard = (SaveBoardObject)serializer.Deserialize(fs);
                Debug.Log("Game data loaded!");

                if (saveBoard.boardHeight > 0 && saveBoard.boardWidth > 0)
                {
                    tileColumns = saveBoard.boardWidth;
                    tileRows = saveBoard.boardHeight;

                    loadEmptyBoard();

                    loadBoardTilesOntoBoard();
                }
                else
                {
                    ShowConfigOptions();
                }

                fs.Close();
            }
            catch
            {
                ShowConfigOptions();
            }
            
        }
        else { 
            Debug.LogError("There is no save data!");
            //loadBoardFresh
        }
    }

    public void ShowConfigOptions()
    {
        gameBoardCanvas.gameObject.SetActive(false);
        levelConifgOptionsCanvas.gameObject.SetActive(true);

        saveConfigButton.GetComponent<Button>().onClick.AddListener(SaveConfigOptions);
    }

    public void SaveConfigOptions()
    {
        tileRows = int.Parse(enteredHeigthInput.text);
        tileColumns = int.Parse(enteredWidthInput.text);

        enteredHeigthInput.text = "";
        enteredWidthInput.text = "";

        loadEmptyBoard();
    }

    public void loadBoardTilesOntoBoard()
    {
        //load edge tiles in
        for (int i = 0; i < saveBoard.edgeTiles.Count; i++)
        {
            var edgeTileToAdd = saveBoard.edgeTiles[i];

            edgeTiles[edgeTileToAdd.side][edgeTileToAdd.index].GetComponent<EdgeTile>().edgeTileAttribute = (GameObject)Instantiate(edgeTiles[edgeTileToAdd.side][edgeTileToAdd.index].GetComponent<EdgeTile>().edgeTileAttributePrefab, new Vector2(0, 0), Quaternion.identity, edgeTiles[edgeTileToAdd.side][edgeTileToAdd.index].GetComponent<EdgeTile>().transform);
            var attributeTile = edgeTiles[edgeTileToAdd.side][edgeTileToAdd.index].GetComponent<EdgeTile>().edgeTileAttribute.GetComponent<TileAttribute>();
            if (attributeTile)
            {
                attributeTile.CopyFromEdgeSave(edgeTileToAdd);
                attributeTile.RotateToFaceCenter(edgeTiles[edgeTileToAdd.side][edgeTileToAdd.index].GetComponent<EdgeTile>().position);
            }

            edgeTiles[edgeTileToAdd.side][edgeTileToAdd.index].GetComponent<EdgeTile>().spriteRenderer.sprite = edgeTiles[edgeTileToAdd.side][edgeTileToAdd.index].GetComponent<EdgeTile>().mySecondSprite;
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