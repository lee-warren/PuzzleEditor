using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAttribute : MonoBehaviour
{

    GameObject thisAttributeTile;
    public RectTransform thisAttributeRectTransform;
    public SpriteRenderer thisAttributeRenderer;

    public string type;
    public int rotation = 0;
    public int colorArrayNumber = -1; //no colour //old
    public LazorColour mainTileColour = new LazorColour("White", new Color(255,255,255,255));

    void Awake()
    {
        thisAttributeTile = GetComponent<GameObject>();
        thisAttributeRectTransform = GetComponent<RectTransform>();
        thisAttributeRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CopyFromEdgeSave(SaveEdgeTile tileToCopy)
    {
        transform.localScale = new Vector3(1, 1, 0);
        transform.localPosition = new Vector2(0, 0);

        colorArrayNumber = tileToCopy.colorArrayNumber;

        mainTileColour = GameBoard.instance.primaryColours.Find(c => c.colourName == tileToCopy.colourName);
        if (mainTileColour == null)
        {
            mainTileColour = GameBoard.instance.secondaryColours.Find(c => c.colourName == tileToCopy.colourName);
        }
        thisAttributeRenderer.color = mainTileColour.colour;

        foreach (Sprite sprite in GameBoard.instance.possibleEdgeAttributes)
        {
            if (sprite.name == tileToCopy.type) {
                thisAttributeRenderer.sprite = sprite;
            }
        }
        type = tileToCopy.type;
    }

    public void CopyFromGameSave(SaveGameTile tileToCopy)
    {
        transform.localScale = new Vector3(1, 1, 0);
        transform.localPosition = new Vector2(0, 0);

        colorArrayNumber = tileToCopy.colorArrayNumber;

        mainTileColour = GameBoard.instance.primaryColours.Find(c => c.colourName == tileToCopy.colourName);
        if (mainTileColour == null)
        {
            mainTileColour = GameBoard.instance.secondaryColours.Find(c => c.colourName == tileToCopy.colourName);
        }
        thisAttributeRenderer.color = mainTileColour.colour;

        foreach (Sprite sprite in GameBoard.instance.possibleAttributes)
        {
            if (sprite.name == tileToCopy.type)
            {
                thisAttributeRenderer.sprite = sprite;
            }
        }
        type = tileToCopy.type;
        rotation = tileToCopy.rotation;
        InitialRotate();
    }

    public void Copy(TileAttribute tileToCopy)
    {
        transform.localScale = new Vector3(1, 1, 0);
        transform.localPosition = new Vector2(0, 0);

        mainTileColour = tileToCopy.mainTileColour;
        thisAttributeRenderer.color = mainTileColour.colour;

        thisAttributeRenderer.sprite = tileToCopy.thisAttributeRenderer.sprite;
        type = tileToCopy.type;
        rotation = tileToCopy.rotation;
        colorArrayNumber = tileToCopy.colorArrayNumber;
    }

    public void PositionInTile(bool isEdgeTile = false)
    {
        transform.localScale = new Vector3(1,1,0);
        transform.localPosition = new Vector2(0,0);

        if (isEdgeTile)
        {
            //rotate the tile so that the sprite faces towards the board
        }
    }

    public void SetSprite(Sprite attributeSprite)
    {
        thisAttributeRenderer.sprite = attributeSprite;

        type = attributeSprite.name;
    }

    public void ShouldTransform()
    {
        if (type == "Filter" || type == "Star"  || type == "LightEmitter" || type == "LightReceiver" || type == "LightReceiverStar")
        {
            CycleColours();
        }
        else if (type == "Reflector" || type == "Refractor" )
        {
            Rotate();
        }
        else 
        {
            //do nothing
        }
    }

    private void CycleColours()
    {
        colorArrayNumber = colorArrayNumber + 1;
        if (colorArrayNumber == 6) // 6 = number of primary and secondary colours added
        {
            colorArrayNumber = 0;
        }

        if (colorArrayNumber < 3)
        {
            mainTileColour = GameBoard.instance.primaryColours[colorArrayNumber];
        }
        else
        {
            mainTileColour = GameBoard.instance.secondaryColours[colorArrayNumber - 3];
        }

        thisAttributeRenderer.color = mainTileColour.colour;
    }

    private void Rotate()
    {
        rotation = rotation + 90;
        transform.Rotate(0, 0, -90);
    }

    private void InitialRotate()
    {
        transform.Rotate(0, 0, -rotation);
    }

    public void RotateToFaceCenter(Vector2 position)
    {
        //This assumes the initial sprite is facing down

        if (position.x == 0)
        {
            rotation = 270;
            transform.Rotate(0, 0, -270);
        } 
        else if (position.x == GameBoard.instance.tileColumns + 1)
        {
            rotation = 90;
            transform.Rotate(0, 0, -90);
        }
        else if (position.y == 0)
        {
            rotation = 180;
            transform.Rotate(0, 0, -180);
        }
        else if (position.y == GameBoard.instance.tileRows + 1)
        {
            rotation = 0;
            //Its already facing the right way. I hope
        }

    }
}
