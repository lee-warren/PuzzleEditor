using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EdgeTile : MonoBehaviour, IPointerClickHandler
{
    public Vector2 position;
    public int side;
    public int index;

    GameObject thisEdgeTile;
    RectTransform thisRectTransform;

    public GameObject edgeTileAttributePrefab;
    public GameObject edgeTileAttribute;

    public SpriteRenderer spriteRenderer;
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

    public void InitialiseForPuzzleGameBoard(int newSide, int newIndex)
    {
        //side is which of the strips it will fall into (top, left, right, bottom)
        //index is which index of that row it will fal into
        side = newSide;
        index = newIndex;

        isBoardEdgeTile = true;
        if (side == 0)// top
        {
            position = new Vector2(index + 1, GameBoard.instance.tileRows + 1);
        }
        else if (side == 1)// left
        {
            position = new Vector2(0, index + 1);
        }
        else if (side == 2)// right
        {
            position = new Vector2(GameBoard.instance.tileColumns + 1, index + 1);
        }
        else if (side == 3)// bottom
        {
            position = new Vector2(index + 1, 0);
        }

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        PositionEdgeTileForPuzzleGameBoard();
        //print(position.x + "," + position.y);
    }

    public void InitialiseForPalette(int columnPosition, Sprite attributeSprite)
    {
        isBoardEdgeTile = false;
        position = new Vector2(1, columnPosition);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        PositionEdgeTileForPalette();

        edgeTileAttribute = new GameObject();
        edgeTileAttribute = (GameObject)Instantiate(edgeTileAttributePrefab, new Vector2(0, 0), Quaternion.identity, transform);
        var attributeEdgeTile = edgeTileAttribute.GetComponent<TileAttribute>();
        if (attributeEdgeTile)
        {
            attributeEdgeTile.PositionInTile(true); //true for being an edge tile
            attributeEdgeTile.SetSprite(attributeSprite);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (isBoardEdgeTile)
        {
            if (GameBoard.instance.selectedEdgeTile)
            {
                bool justRemove = false;
                if (edgeTileAttribute && edgeTileAttribute.GetComponent<TileAttribute>().type == GameBoard.instance.selectedEdgeTile.type)
                {
                    justRemove = true;
                }

                if (transform.childCount >= 1) //kills all existing children to make way for the new attribute
                {
                    foreach (Transform child in transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                }

                if (justRemove)
                {
                    spriteRenderer.sprite = myFirstSprite;
                }
                else
                {
                    edgeTileAttribute = (GameObject)Instantiate(edgeTileAttributePrefab, new Vector2(0, 0), Quaternion.identity, transform);
                    var attributeTile = edgeTileAttribute.GetComponent<TileAttribute>();
                    if (attributeTile)
                    {
                        attributeTile.Copy(GameBoard.instance.selectedEdgeTile);
                        attributeTile.RotateToFaceCenter(position);
                    }

                    spriteRenderer.sprite = mySecondSprite;
                }
            }
            else // no tile in the palette is selected
            {
                if (edgeTileAttribute && GameBoard.instance.selectedEditOptionsTile)
                {
                    if (GameBoard.instance.selectedEditOptionsTile.type == "Rotate") //this should always do nothing because edge tile shouldn't rotate
                    {
                        edgeTileAttribute.GetComponent<TileAttribute>().ShouldTransformRotate();
                    }
                    else if (GameBoard.instance.selectedEditOptionsTile.type == "Colour")
                    {
                        edgeTileAttribute.GetComponent<TileAttribute>().ShouldTransformColour();
                    }
                    else if (GameBoard.instance.selectedEditOptionsTile.type == "Lock")
                    {
                        edgeTileAttribute.GetComponent<TileAttribute>().ShouldTransformLock();
                    }
                }
            }
        }
        else if (!isBoardEdgeTile)
        {
            if (GameBoard.instance.selectedEdgeTile == edgeTileAttribute.GetComponent<TileAttribute>())
            {
                GameBoard.instance.selectedEdgeTile = new TileAttribute();
                spriteRenderer.sprite = myFirstSprite;
            }
            else
            {
                //deselect all the other palette tiles:
                foreach (GameObject child in GameBoard.instance.edgePaletteTiles)
                {
                    child.GetComponent<EdgeTile>().spriteRenderer.sprite = myFirstSprite;
                }

                GameBoard.instance.selectedEdgeTile = edgeTileAttribute.GetComponent<TileAttribute>();
                GameBoard.instance.selectedEdgeTile.Copy(edgeTileAttribute.GetComponent<TileAttribute>());
                spriteRenderer.sprite = mySecondSprite;
            }
        }
    }

    private void PositionEdgeTileForPuzzleGameBoard()
    {//sketchy AF... be carefull changing anything in here

        RectTransform parentsRect = thisRectTransform.parent.GetComponent<RectTransform>();
        Vector2 bottomLeftCorner = new Vector2(-parentsRect.sizeDelta.x / 2, -parentsRect.sizeDelta.y / 2);

        //sets size of the square relevant to the parents size
        if (GameBoard.instance.tileColumns > GameBoard.instance.tileRows)
        {
            transform.localScale = new Vector3((1 - 1 * spacing) / (float)(GameBoard.instance.tileColumns + 2) - spacing, (1 - 1 * spacing) / (float)(GameBoard.instance.tileColumns + 2) - spacing, 0);
        }
        else
        {
            transform.localScale = new Vector3((1 - 1 * spacing) / (float)(GameBoard.instance.tileRows + 2) - spacing, (1 - 1 * spacing) / (float)(GameBoard.instance.tileRows + 2) - spacing, 0);
        }

        Vector2 fakeBottomLeftCorner = new Vector2(-parentsRect.sizeDelta.x * (spacing + transform.localScale.x / 2) - ((float)(GameBoard.instance.tileColumns + 1) / 2.0f) * parentsRect.sizeDelta.x * (spacing + transform.localScale.x), -parentsRect.sizeDelta.y * (spacing + transform.localScale.y / 2) - ((float)(GameBoard.instance.tileRows + 1) / 2.0f) * parentsRect.sizeDelta.y * (spacing + transform.localScale.y));

        transform.localPosition = new Vector2(fakeBottomLeftCorner.x + parentsRect.sizeDelta.x * (spacing + transform.localScale.x / 2) + position.x * parentsRect.sizeDelta.x * (spacing + transform.localScale.x), fakeBottomLeftCorner.y + parentsRect.sizeDelta.y * (spacing + transform.localScale.y / 2) + position.y * parentsRect.sizeDelta.y * (spacing + transform.localScale.y));
    }

    private void PositionEdgeTileForPalette()
    {
        RectTransform parentsRect = thisRectTransform.parent.GetComponent<RectTransform>();
        Vector2 bottomLeftCorner = new Vector2(-parentsRect.sizeDelta.x / 2, -parentsRect.sizeDelta.y / 2);

        //sets size of the square relevant to the parents size
        if (GameBoard.instance.tileColumns > GameBoard.instance.tileRows)
        {
            transform.localScale = new Vector3((((1 - 1 * spacing) / (float)(GameBoard.instance.tileColumns + 2) - spacing)) * parentsRect.sizeDelta.y / parentsRect.sizeDelta.x, (1 - 1 * spacing) / (float)(GameBoard.instance.tileColumns + 2) - spacing, 0);
            transform.localPosition = new Vector2(bottomLeftCorner.x + parentsRect.sizeDelta.x * (spacing + transform.localScale.x / 2) + position.x * parentsRect.sizeDelta.x * (spacing + transform.localScale.x), bottomLeftCorner.y + parentsRect.sizeDelta.y * (spacing + transform.localScale.y / 2) + (GameBoard.instance.tileColumns + 1 - position.y) * parentsRect.sizeDelta.y * (spacing + transform.localScale.y));
        }
        else
        {
            transform.localScale = new Vector3((((1 - 1 * spacing) / (float)(GameBoard.instance.tileRows + 2) - spacing)) * parentsRect.sizeDelta.y / parentsRect.sizeDelta.x, (1 - 1 * spacing) / (float)(GameBoard.instance.tileRows + 2) - spacing, 0);
            transform.localPosition = new Vector2(bottomLeftCorner.x + parentsRect.sizeDelta.x * (spacing + transform.localScale.x / 2) + position.x * parentsRect.sizeDelta.x * (spacing + transform.localScale.x), bottomLeftCorner.y + parentsRect.sizeDelta.y * (spacing + transform.localScale.y / 2) + (GameBoard.instance.tileRows + 1 - position.y) * parentsRect.sizeDelta.y * (spacing + transform.localScale.y));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
