using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public GameObject puzzleGameBoard;
    public GameObject pallette;

    public int tileColumns = 1;
    public int tileRows = 1;
    public float tileSpacing = 0.1f;
    public Vector2 distanceFromCenterToEdge;
    public Vector2 screenSize;
    public Vector2 bottomLeftCorner;
    
    public GameObject tilePrefab;
    public GameObject[,] tiles;
    public GameObject[] palletteTiles;

    public Tile selectedTile;

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
        //distanceFromCenterToEdge = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        //screenSize = new Vector2(distanceFromCenterToEdge.x*2, distanceFromCenterToEdge.y*2);
        //bottomLeftCorner = new Vector2(-distanceFromCenterToEdge.x, -distanceFromCenterToEdge.y);

        //This will hold all of the information on what should go on the screen

        //initialise the pallete, and all of the squares to go in there
        palletteTiles = new GameObject[tileColumns];
        for (int j = 0; j < tileColumns; j++)
        {
            palletteTiles[j] = (GameObject)Instantiate(tilePrefab, new Vector2(0, 0), Quaternion.identity, pallette.transform);
            var tile = palletteTiles[j].GetComponent<Tile>();
            if (tile)
            {
                tile.InitialiseForPalette(new Vector2(1, j));
            }
            //Debug.Log("Tile Height : " + tiles[i,j].transform.localScale.x);

        }

        //initialise all of the empty squares from the tile prefab to go in the game board:
        tiles = new GameObject[tileRows,tileColumns];
        for (int i =0; i< tileRows; i++)
        {
            for (int j =0; j< tileColumns; j++)
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
