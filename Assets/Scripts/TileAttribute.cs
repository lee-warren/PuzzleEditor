using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAttribute : MonoBehaviour
{

    GameObject thisAttributeTile;
    public RectTransform thisAttributeRectTransform;
    public SpriteRenderer thisAttributeRenderer;

    private string type;
    private int rotation = 0;
    private int colorArrayNumber = -1; //no colour

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

    public void Copy(TileAttribute tileToCopy)
    {
        transform.localScale = new Vector3(1, 1, 0);
        transform.localPosition = new Vector2(0, 0);

        thisAttributeRenderer.color = tileToCopy.thisAttributeRenderer.color;
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
        if (type == "Filter" || type == "LightEmitter" || type == "LightReceiver")
        {
            CycleColours();
        }
        else
        {
            Rotate();
        }
    }

    private void CycleColours()
    {
        colorArrayNumber = colorArrayNumber + 1;
        if (colorArrayNumber == GameBoard.instance.possibleColours.Length)
        {
            colorArrayNumber = 0;
        }

        thisAttributeRenderer.color = new Color(GameBoard.instance.possibleColours[colorArrayNumber].r/255f, GameBoard.instance.possibleColours[colorArrayNumber].g/255f, GameBoard.instance.possibleColours[colorArrayNumber].b/255, 1);
    }

    private void Rotate()
    {
        rotation = rotation + 90;
        transform.Rotate(0, 0, -90);
    }

    public void RotateToFaceCenter(Vector2 position)
    {
        //This assumes the initial sprite is facing down

        if (position.x == 0)
        {
            rotation = 270;
            transform.Rotate(0, 0, -270);
        } 
        else if (position.x == GameBoard.instance.tileRowColumns + 1)
        {
            rotation = 90;
            transform.Rotate(0, 0, -90);
        }
        else if (position.y == 0)
        {
            rotation = 180;
            transform.Rotate(0, 0, -180);
        }
        else if (position.y == GameBoard.instance.tileRowColumns + 1)
        {
            rotation = 0;
            //Its already facing the right way. I hope
        }

    }
}
