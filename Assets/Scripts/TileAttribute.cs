using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAttribute : MonoBehaviour
{

    GameObject thisAttributeTile;
    public RectTransform thisAttributeRectTransform;
    public SpriteRenderer thisAttributeRenderer;

    //specific to edge tiles
    public GameObject edgeTileArrow;

    //specific to game tiles
    public GameObject leftRectangleWithArrow;
    public GameObject topRectangleWithArrow;
    public GameObject rightRectangleWithArrow;
    public GameObject bottomRectangleWithArrow;
    public GameObject leftArrow;
    public GameObject topArrow;
    public GameObject rightArrow;
    public GameObject bottomArrow;

    public Sprite rainbowRectangleSprite;

    public string type;
    public int rotation = 0;
    public int colorArrayNumber = -1; //no colour //old
    public LazorColour mainTileColour = new LazorColour("White", new Color(255,255,255,255));

    public bool isLocked = false;
    public GameObject lockedImage;

    void Awake()
    {
        thisAttributeTile = GetComponent<GameObject>();
        thisAttributeRectTransform = GetComponent<RectTransform>();
        //thisAttributeRenderer = GetComponent<SpriteRenderer>();
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

        foreach (Sprite sprite in GameBoard.instance.possibleEdgeAttributes)
        {
            if (sprite.name == tileToCopy.type) {
                SetSprite(sprite);
            }
        }
        type = tileToCopy.type;

        if (type == "LightReceiver" || type == "LightReceiverStar")
        {
            //flip the arrow so it points in
            edgeTileArrow.transform.Rotate(180, 0, 0);
        }

        SetColour();

        lockedImage.SetActive(isLocked);
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
        if (mainTileColour == null)
        {
            mainTileColour = new LazorColour("White", new Color(255, 255, 255, 255));
        }

        foreach (Sprite sprite in GameBoard.instance.possibleAttributes)
        {
            if (sprite.name == tileToCopy.type)
            {
                SetSprite(sprite);
            }
        }

        type = tileToCopy.type;
        rotation = tileToCopy.rotation;
        isLocked = tileToCopy.isLocked;

        InitialRotate();
        SetColour();

        lockedImage.SetActive(isLocked);
    }

    public void Copy(TileAttribute tileToCopy)
    {
        transform.localScale = new Vector3(1, 1, 0);
        transform.localPosition = new Vector2(0, 0);

        mainTileColour = tileToCopy.mainTileColour;
        thisAttributeRenderer.color = mainTileColour.colour;

        thisAttributeRenderer.sprite = tileToCopy.thisAttributeRenderer.sprite;

        //stuff for edge tiles:
        if (tileToCopy.edgeTileArrow)
        {
            edgeTileArrow.transform.rotation = tileToCopy.edgeTileArrow.transform.rotation;
        }

        //stuff for game tiles:
        if (tileToCopy.leftRectangleWithArrow && tileToCopy.topRectangleWithArrow && tileToCopy.rightRectangleWithArrow && tileToCopy.bottomRectangleWithArrow)
        {
            leftRectangleWithArrow.SetActive(tileToCopy.leftRectangleWithArrow.activeSelf);
            topRectangleWithArrow.SetActive(tileToCopy.topRectangleWithArrow.activeSelf);
            rightRectangleWithArrow.SetActive(tileToCopy.rightRectangleWithArrow.activeSelf);
            bottomRectangleWithArrow.SetActive(tileToCopy.bottomRectangleWithArrow.activeSelf);

            //for the rainbow sprite on the bottom rectangle
            bottomRectangleWithArrow.GetComponent<SpriteRenderer>().sprite = tileToCopy.bottomRectangleWithArrow.GetComponent<SpriteRenderer>().sprite;

        }

        type = tileToCopy.type;
        rotation = tileToCopy.rotation;
        colorArrayNumber = tileToCopy.colorArrayNumber;
        isLocked = tileToCopy.isLocked;

        if (lockedImage)
        {
            lockedImage.SetActive(isLocked);
        }
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

        if (type == "LightReceiver" || type == "LightReceiverStar")
        {
            //flip the arrow so it points in
            edgeTileArrow.transform.Rotate(180, 0, 0);
        }

        if (type == "Split")
        {
            bottomRectangleWithArrow.SetActive(true);
            leftRectangleWithArrow.SetActive(true);
            leftArrow.transform.Rotate(0, 0, 180);
            rightRectangleWithArrow.SetActive(true);
            rightArrow.transform.Rotate(0, 0, 180);
        }

        if (type == "Merge")
        {
            topRectangleWithArrow.SetActive(true);
            topArrow.transform.Rotate(0, 0, 180);
            leftRectangleWithArrow.SetActive(true);
            rightRectangleWithArrow.SetActive(true);
        }

        if (type == "Transform")
        {
            topRectangleWithArrow.SetActive(true);
            topArrow.transform.Rotate(0, 0, 180);
            bottomRectangleWithArrow.SetActive(true);
            bottomRectangleWithArrow.GetComponent<SpriteRenderer>().sprite = rainbowRectangleSprite;
        }
    }

    public void SetColour()
    {
        //colour the relevant bits:
        if (type == "Star" || type == "LightEmitter" || type == "LightReceiver" || type == "LightReceiverStar")
        {
            thisAttributeRenderer.color = mainTileColour.colour;
        }
        else if (type == "Transform")
        {
            topRectangleWithArrow.GetComponent<SpriteRenderer>().color = mainTileColour.colour;
        }
        else if (type == "Split")
        {
            bottomRectangleWithArrow.GetComponent<SpriteRenderer>().color = mainTileColour.colour;
            leftRectangleWithArrow.GetComponent<SpriteRenderer>().color = mainTileColour.composition[0].colour;
            rightRectangleWithArrow.GetComponent<SpriteRenderer>().color = mainTileColour.composition[1].colour;

        }
        else if (type == "Merge")
        {
            topRectangleWithArrow.GetComponent<SpriteRenderer>().color = mainTileColour.colour;
            leftRectangleWithArrow.GetComponent<SpriteRenderer>().color = mainTileColour.composition[0].colour;
            rightRectangleWithArrow.GetComponent<SpriteRenderer>().color = mainTileColour.composition[1].colour;
        }
    }

    public void ShouldTransformColour()
    {
        if (type == "Transform" || type == "Split" || type == "Merge" || type == "Star" || type == "LightEmitter" || type == "LightReceiver" || type == "LightReceiverStar")
        {
            CycleColours();
        }
        else 
        {
            //do nothing
        }
    }

    public void ShouldTransformRotate()
    {
        if (type == "Reflector" || type == "Refractor" || type == "Transform" || type == "Split" || type == "Merge" || type == "Star")
        {
            Rotate();
        }
        else
        {
            //do nothing
        }
    }

    public void ShouldTransformLock()
    {
        //everything can be locked/unlocked at the moment
        LockUnlock();
    }

    private void CycleColours()
    {
        colorArrayNumber = colorArrayNumber + 1;

        if (type == "Transform" || type == "Star" || type == "LightEmitter" || type == "LightReceiver" || type == "LightReceiverStar")
        {
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
        }
        else if (type == "Split" || type == "Merge")
        {
            if (colorArrayNumber == 3) // 6 = number of primary and secondary colours added
            {
                colorArrayNumber = 0;
            }

            mainTileColour = GameBoard.instance.secondaryColours[colorArrayNumber];
        }

        SetColour();

    }

    private void Rotate()
    {
        rotation = rotation + 90;
        transform.Rotate(0, 0, -90);

        //locked image rotates the opposite way so it always stay upright
        lockedImage.transform.Rotate(0, 0, 90);

    }

    private void InitialRotate()
    {
        transform.Rotate(0, 0, -rotation);

        //locked image rotates the opposite way so it always stay upright
        lockedImage.transform.Rotate(0, 0, rotation);
    }

    public void RotateToFaceCenter(Vector2 position)
    {
        //This assumes the initial sprite is facing down

        if (position.x == 0)
        {
            rotation = 270;
            transform.Rotate(0, 0, -270);

            //locked image rotates the opposite way so it always stay upright
            lockedImage.transform.Rotate(0, 0, rotation);
        } 
        else if (position.x == GameBoard.instance.tileColumns + 1)
        {
            rotation = 90;
            transform.Rotate(0, 0, -90);

            //locked image rotates the opposite way so it always stay upright
            lockedImage.transform.Rotate(0, 0, rotation);
        }
        else if (position.y == 0)
        {
            rotation = 180;
            transform.Rotate(0, 0, -180);

            //locked image rotates the opposite way so it always stay upright
            lockedImage.transform.Rotate(0, 0, rotation);
        }
        else if (position.y == GameBoard.instance.tileRows + 1)
        {
            rotation = 0;
            //Its already facing the right way. I hope
        }

    }

    private void LockUnlock()
    {
        isLocked = !isLocked;

        lockedImage.SetActive(isLocked);
    }
}
