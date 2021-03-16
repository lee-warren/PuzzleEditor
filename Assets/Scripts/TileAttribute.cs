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

    public void PositionInTile()
    {
        transform.localScale = new Vector3(1,1,0);
        transform.localPosition = new Vector2(0,0);
    }

    public void SetSprite(Sprite attributeSprite)
    {
        thisAttributeRenderer.sprite = attributeSprite;

        type = attributeSprite.name;
    }

    public void ShouldTransform()
    {
        if (type == "Filter")
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


    public void RandomColour()
    {
        thisAttributeRenderer.color = new Color(transform.parent.position.y, transform.parent.position.y, transform.parent.position.y, 1f); 
    }
}
