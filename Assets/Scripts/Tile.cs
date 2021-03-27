using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler 
{
    public Vector2 position;
    GameObject thisTile;
    RectTransform thisRectTransform;

    public GameObject tileAttributePrefab;
    public GameObject tileAttribute;

    public SpriteRenderer spriteRenderer;
    public Sprite myFirstSprite;
    public Sprite mySecondSprite;

    private float spacing = 1f / 100f;

    private bool isBoardTile = true;

    // Start is called before the first frame update
    void Awake()
    {
      thisTile = GetComponent<GameObject>();
      thisRectTransform = GetComponent<RectTransform>();
    }

    public void InitialiseForPuzzleGameBoard(Vector2 rowColumn)
    {
        isBoardTile = true;
        position = rowColumn;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();


        PositionTileForPuzzleGameBoard();
        //thisTile.transform.position = new Vector3(position.x, position.y, 0);
    }

    public void InitialiseForPalette(int column, Sprite attributeSprite)
    {
        isBoardTile = false;
        position = new Vector2(1, column);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        PositionTileForPalette();

        tileAttribute = (GameObject)Instantiate(tileAttributePrefab, new Vector2(0, 0), Quaternion.identity, transform);
        var attributeTile = tileAttribute.GetComponent<TileAttribute>();
        if (attributeTile)
        {
            attributeTile.PositionInTile();
            attributeTile.SetSprite(attributeSprite);
        }
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {      
        //print("I was clicked (" + position.x + "," + position.y + ")");

        if (isBoardTile)
        {
            if (GameBoard.instance.selectedTile)
            {
                bool justRemove = false;
                if (tileAttribute && tileAttribute.GetComponent<TileAttribute>().type == GameBoard.instance.selectedTile.type)
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
                else {
                    tileAttribute = (GameObject)Instantiate(tileAttributePrefab, new Vector2(0, 0), Quaternion.identity, transform);
                    var attributeTile = tileAttribute.GetComponent<TileAttribute>();
                    if (attributeTile)
                    {
                        attributeTile.Copy(GameBoard.instance.selectedTile);
                    }

                    spriteRenderer.sprite = mySecondSprite;
                }
            }
            else // no tile in the palette is selected
            {
                if (tileAttribute)
                {
                    tileAttribute.GetComponent<TileAttribute>().ShouldTransform(); //This will change the colour of rotate depending on the attributes type
                }
            }
        }
        else if (!isBoardTile)
        {
            if (GameBoard.instance.selectedTile == tileAttribute.GetComponent<TileAttribute>())
            {
                GameBoard.instance.selectedTile = new TileAttribute();
                spriteRenderer.sprite = myFirstSprite;
            }
            else
            {
                //deselect all the other palette tiles:
                foreach (GameObject child in GameBoard.instance.paletteTiles)
                {
                    child.GetComponent<Tile>().spriteRenderer.sprite = myFirstSprite;
                }

                GameBoard.instance.selectedTile = tileAttribute.GetComponent<TileAttribute>();
                spriteRenderer.sprite = mySecondSprite;
            }
        }
    }

    private void PositionTileForPuzzleGameBoard() {//sketchy AF... be carefull changing anything in here

        RectTransform parentsRect = thisRectTransform.parent.GetComponent<RectTransform>();
        Vector2 bottomLeftCorner = new Vector2(- parentsRect.sizeDelta.x/2, - parentsRect.sizeDelta.y/2);

        //sets size of the square relevant to the parents size
        transform.localScale = new Vector3((1 - 1*spacing)/(float)(GameBoard.instance.tileRows + 2) - spacing,(1 - 1*spacing)/(float)(GameBoard.instance.tileColumns + 2) - spacing,0);

        var boarderTileBoxWidth = spacing + transform.localScale.x;
        transform.localPosition = new Vector2(bottomLeftCorner.x + parentsRect.sizeDelta.x*(spacing + transform.localScale.x/2 + boarderTileBoxWidth) + position.x * parentsRect.sizeDelta.x * (spacing + transform.localScale.x), bottomLeftCorner.y + parentsRect.sizeDelta.y * (spacing + transform.localScale.y/ 2 + boarderTileBoxWidth) + position.y * parentsRect.sizeDelta.y * (spacing + transform.localScale.y));
    }

    private void PositionTileForPalette()
    {
        RectTransform parentsRect = thisRectTransform.parent.GetComponent<RectTransform>();
        Vector2 bottomLeftCorner = new Vector2(-parentsRect.sizeDelta.x / 2, -parentsRect.sizeDelta.y / 2);

        //sets size of the square relevant to the parents size
        transform.localScale = new Vector3((((1 - 1 * spacing) / (float)(GameBoard.instance.tileRows + 2) - spacing)) * parentsRect.sizeDelta.y / parentsRect.sizeDelta.x, (1 - 1 * spacing) / (float)(GameBoard.instance.tileColumns + 2) - spacing, 0);

        transform.localPosition = new Vector2(bottomLeftCorner.x + parentsRect.sizeDelta.x * (spacing + transform.localScale.x / 2) + position.x * parentsRect.sizeDelta.x * (spacing + transform.localScale.x), bottomLeftCorner.y + parentsRect.sizeDelta.y * (spacing + transform.localScale.y / 2) + (GameBoard.instance.tileColumns + 1 - position.y) * parentsRect.sizeDelta.y * (spacing + transform.localScale.y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
