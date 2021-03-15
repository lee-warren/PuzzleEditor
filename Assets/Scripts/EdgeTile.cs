using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EdgeTile : MonoBehaviour, IPointerClickHandler
{
    public Vector2 position;
    GameObject thisEdgeTile;
    RectTransform thisRectTransform;

    public GameObject edgeTileAttributePrefab;
    public GameObject edgeTileAttribute;

    SpriteRenderer spriteRenderer;
    public Sprite myFirstSprite;
    public Sprite mySecondSprite;

    private float spacing = 1f / 100f;

    private bool isBoardEdgeTile = true;

    // Start is called before the first frame update
    void Awake()
    {
        thisEdgeTile = GetComponent<GameObject>();
        thisRectTransform = GetComponent<RectTransform>();
    }

    public void InitialiseForPuzzleGameBoard(int side, int index)
    {
        //rowColumn = 
            //row is which of the strips it will fall into (top, left, right, bottom)
            //column is which index of that row it will fal into

        isBoardEdgeTile = true;
        if (side == 0)// top
        {
            position = new Vector2(index + 1, GameBoard.instance.tileRowColumns + 1);
        }
        else if (side == 1)// left
        {
            position = new Vector2(0, index + 1);
        }
        else if (side == 2)// right
        {
            position = new Vector2(GameBoard.instance.tileRowColumns + 1, index + 1);
        }
        else if (side == 3)// bottom
        {
            position = new Vector2(index + 1, 0);
        }

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        PositionEdgeTileForPuzzleGameBoard();
        print(position.x + "," + position.y);
    }

    public void InitialiseForPalette(int columnPosition)
    {
        isBoardEdgeTile = false;
        position = new Vector2(1, columnPosition);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        PositionEdgeTileForPalette();
        print(position.x + "," + position.y);
        /*
        edgeTileAttribute = new GameObject();
        edgeTileAttribute = (GameObject)Instantiate(edgeTileAttributePrefab, new Vector2(0, 0), Quaternion.identity, transform);
        var attributeEdgeTile = edgeTileAttribute.GetComponent<EdgeTileAttribute>();
        if (attributeEdgeTile)
        {
            attributeEdgeTile.PositionInEdgeTile();
            attributeEdgeTile.RandomColour();
        }
        */
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("I was clicked (" + position.x + "," + position.y + ")");
        if (spriteRenderer.sprite == mySecondSprite)
        {
            spriteRenderer.sprite = myFirstSprite;
        }
        else
        {
            spriteRenderer.sprite = mySecondSprite;
        }
        /*

        if (isBoardEdgeTile && GameBoard.instance.selectedEdgeTile)
        {
            edgeTileAttribute = new GameObject();
            edgeTileAttribute = (GameObject)Instantiate(edgeTileAttributePrefab, new Vector2(0, 0), Quaternion.identity, transform);
            var attributeEdgeTile = edgeTileAttribute.GetComponent<EdgeTileAttribute>();
            if (attributeEdgeTile)
            {
                attributeEdgeTile.Copy(GameBoard.instance.selectedEdgeTile.GetComponent<EdgeTileAttribute>());
            }
        }
        else if (!isBoardEdgeTile)
        {
            GameBoard.instance.selectedEdgeTile = new GameObject();
            GameBoard.instance.selectedEdgeTile = (GameObject)Instantiate(edgeTileAttributePrefab, new Vector2(0, 0), Quaternion.identity, transform);
            var attributeEdgeTile = GameBoard.instance.selectedEdgeTile.GetComponent<EdgeTileAttribute>();
            if (attributeEdgeTile)
            {
                attributeEdgeTile.Copy(edgeTileAttribute.GetComponent<EdgeTileAttribute>());
            }
        }
        */
    }

    private void PositionEdgeTileForPuzzleGameBoard()
    {//sketchy AF... be carefull changing anything in here

        RectTransform parentsRect = thisRectTransform.parent.GetComponent<RectTransform>();
        Vector2 bottomLeftCorner = new Vector2(-parentsRect.sizeDelta.x / 2, -parentsRect.sizeDelta.y / 2);
        print(bottomLeftCorner);
        print(thisRectTransform.sizeDelta.x);

        //sets size of the square relevant to the parents size
        transform.localScale = new Vector3((1 - 1 * spacing) / (float)(GameBoard.instance.tileRowColumns + 2) - spacing, (1 - 1 * spacing) / (float)(GameBoard.instance.tileRowColumns + 2) - spacing, 0);

        transform.localPosition = new Vector2(bottomLeftCorner.x + parentsRect.sizeDelta.x * (spacing + transform.localScale.x / 2) + position.x * parentsRect.sizeDelta.x * (spacing + transform.localScale.x), bottomLeftCorner.y + parentsRect.sizeDelta.y * (spacing + transform.localScale.y / 2) + position.y * parentsRect.sizeDelta.y * (spacing + transform.localScale.y));
    }

    private void PositionEdgeTileForPalette()
    {
        RectTransform parentsRect = thisRectTransform.parent.GetComponent<RectTransform>();
        Vector2 bottomLeftCorner = new Vector2(-parentsRect.sizeDelta.x / 2, -parentsRect.sizeDelta.y / 2);
        print(bottomLeftCorner);
        print(thisRectTransform.sizeDelta.x);

        //sets size of the square relevant to the parents size
        transform.localScale = new Vector3((((1 - 1 * spacing) / (float)(GameBoard.instance.tileRowColumns + 2) - spacing))* parentsRect.sizeDelta.y/ parentsRect.sizeDelta.x, (1 - 1 * spacing) / (float)(GameBoard.instance.tileRowColumns + 2) - spacing, 0);

        transform.localPosition = new Vector2(bottomLeftCorner.x + parentsRect.sizeDelta.x * (spacing + transform.localScale.x / 2) + position.x * parentsRect.sizeDelta.x * (spacing + transform.localScale.x), bottomLeftCorner.y + parentsRect.sizeDelta.y * (spacing + transform.localScale.y / 2) + position.y * parentsRect.sizeDelta.y * (spacing + transform.localScale.y));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
