using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
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
    public GameObject[,] edgeTiles;
    public GameObject[] edgePaletteTiles;

    public Color[] possibleColours;

    public TileAttribute selectedTile;
    public GameObject selectedEdgeTile;

    public static GameBoard instance;            //A reference to our game control script so we can access it statically.
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
        //resize the palette
        //to-do

        //initialise the edgePalette
        //initialise the pallete, and all of the squares to go in there
        edgePaletteTiles = new GameObject[tileRowColumns + 2];
        for (int j = 0; j < tileRowColumns + 2; j++)
        {
            edgePaletteTiles[j] = (GameObject)Instantiate(edgeTilePrefab, new Vector2(0, 0), Quaternion.identity, edgePalette.transform);
            var tile = edgePaletteTiles[j].GetComponent<EdgeTile>();
            if (tile)
            {
                tile.InitialiseForPalette(j);
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
                //Debug.Log("Tile Height : " + tiles[i,j].transform.localScale.x);

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

    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
