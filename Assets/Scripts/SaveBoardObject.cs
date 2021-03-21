using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveBoardObject
{
    public int boardLength;

    public List<SaveEdgeTile> edgeTiles;
    public List<SaveGameTile> gameTiles;


    public SaveBoardObject()
    {
    }

    public SaveBoardObject(int boardLength)
    {
        this.boardLength = boardLength;
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
}

[Serializable]
public class SaveGameTile
{
    public int positionX;
    public int positionY;

    public string type;
    public int rotation = 0;
    public int colorArrayNumber = -1; //no colour
    public bool isLocked;
}
