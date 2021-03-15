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

    SpriteRenderer spriteRenderer;
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
        print(position.x + "," + position.y);
    }

    public void InitialiseForPalette(Vector2 rowColumn)
    {
        isBoardTile = false;
        position = rowColumn;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        PositionTileForPalette();
        print(position.x + "," + position.y);

        tileAttribute = new GameObject();
        tileAttribute = (GameObject)Instantiate(tileAttributePrefab, new Vector2(0, 0), Quaternion.identity, transform);
        var attributeTile = tileAttribute.GetComponent<TileAttribute>();
        if (attributeTile)
        {
            attributeTile.PositionInTile();
            attributeTile.RandomColour();
        }
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {      
        print("I was clicked (" + position.x + "," + position.y + ")");
        if (spriteRenderer.sprite == mySecondSprite) {
            spriteRenderer.sprite = myFirstSprite;
        } else {
            spriteRenderer.sprite = mySecondSprite;
        }

        if (isBoardTile && GameBoard.instance.selectedTile)
        {
            tileAttribute = new GameObject();
            tileAttribute = (GameObject)Instantiate(tileAttributePrefab, new Vector2(0, 0), Quaternion.identity, transform);
            var attributeTile = tileAttribute.GetComponent<TileAttribute>();
            if (attributeTile)
            {
                attributeTile.Copy(GameBoard.instance.selectedTile.GetComponent<TileAttribute>());
            }
        }
        else if (!isBoardTile)
        {
            GameBoard.instance.selectedTile = new GameObject();
            GameBoard.instance.selectedTile = (GameObject)Instantiate(tileAttributePrefab, new Vector2(0, 0), Quaternion.identity, transform);
            var attributeTile = GameBoard.instance.selectedTile.GetComponent<TileAttribute>();
            if (attributeTile)
            {
                attributeTile.Copy(tileAttribute.GetComponent<TileAttribute>());
            }
        }
    }

private void PositionTileForPuzzleGameBoard() {//sketchy AF... be carefull changing anything in here

        RectTransform parentsRect = thisRectTransform.parent.GetComponent<RectTransform>();
        Vector2 bottomLeftCorner = new Vector2(- parentsRect.sizeDelta.x/2, - parentsRect.sizeDelta.y/2);
        print(bottomLeftCorner);
        print(thisRectTransform.sizeDelta.x);

        //sets size of the square relevant to the parents size
        transform.localScale = new Vector3((1 - 1*spacing)/(float)(GameBoard.instance.tileRowColumns + 2) - spacing,(1 - 1*spacing)/(float)(GameBoard.instance.tileRowColumns + 2) - spacing,0);

        var boarderTileBoxWidth = spacing + transform.localScale.x;
        transform.localPosition = new Vector2(bottomLeftCorner.x + parentsRect.sizeDelta.x*(spacing + transform.localScale.x/2 + boarderTileBoxWidth) + position.x * parentsRect.sizeDelta.x * (spacing + transform.localScale.x), bottomLeftCorner.y + parentsRect.sizeDelta.y * (spacing + transform.localScale.y/ 2 + boarderTileBoxWidth) + position.y * parentsRect.sizeDelta.y * (spacing + transform.localScale.y));
    }

    private void PositionTileForPalette()
    {
        RectTransform parentsRect = thisRectTransform.parent.GetComponent<RectTransform>();
        Vector2 bottomLeftCorner = new Vector2(-parentsRect.sizeDelta.x / 2, -parentsRect.sizeDelta.y / 2);
        print(bottomLeftCorner);
        print(thisRectTransform.sizeDelta.x);

        //sets size of the square relevant to the parents size
        transform.localScale = new Vector3((((1 - 1 * spacing) / (float)(GameBoard.instance.tileRowColumns + 2) - spacing)) * parentsRect.sizeDelta.y / parentsRect.sizeDelta.x, (1 - 1 * spacing) / (float)(GameBoard.instance.tileRowColumns + 2) - spacing, 0);

        transform.localPosition = new Vector2(bottomLeftCorner.x + parentsRect.sizeDelta.x * (spacing + transform.localScale.x / 2) + position.x * parentsRect.sizeDelta.x * (spacing + transform.localScale.x), bottomLeftCorner.y + parentsRect.sizeDelta.y * (spacing + transform.localScale.y / 2) + position.y * parentsRect.sizeDelta.y * (spacing + transform.localScale.y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
