using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveBoardObject
{
    public int boardHeight;
    public int boardWidth;

    public List<SaveEdgeTile> edgeTiles;
    public List<SaveGameTile> gameTiles;


    public SaveBoardObject()
    {
    }

    public SaveBoardObject(int boardWidth, int boardHeight)
    {
        this.boardHeight = boardHeight;
        this.boardWidth = boardWidth;

        this.edgeTiles = new List<SaveEdgeTile>();

        this.gameTiles = new List<SaveGameTile>();
    }

}

[Serializable]
public class SaveEdgeTile
{
    public int side; //side is which of the strips it will fall into (top, left, right, bottom)
    public int index; //index is which index of that row it will fall into

    public string type;
    public int colorArrayNumber = -1; //no colour
    public string colourName;
    public bool isLocked = true;
}

[Serializable]
public class SaveGameTile
{
    public int positionX;
    public int positionY;

    public string type;
    public int rotation = 0;
    public int colorArrayNumber = -1; //no colour
    public string colourName;
    public bool isLocked;
}
