using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditOptionsTile : MonoBehaviour, IPointerClickHandler
{
    public Vector2 position;

    GameObject thisEditOptionsTile;
    RectTransform thisRectTransform;

    public GameObject editOptionsTileAttributePrefab;
    public GameObject editOptionsTileAttribute;

    public SpriteRenderer spriteRenderer;
    public Sprite myFirstSprite;
    public Sprite mySecondSprite;

    private float spacing = 1f / 100f;


    // Start is called before the first frame update
    void Awake()
    {
        thisEditOptionsTile = GetComponent<GameObject>();
        thisRectTransform = GetComponent<RectTransform>();
    }

    public void InitialiseForPalette(int columnPosition, Sprite attributeSprite)
    {
        position = new Vector2(1, columnPosition);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        PositionEditOptionsTileForPalette();

        editOptionsTileAttribute = new GameObject();
        editOptionsTileAttribute = (GameObject)Instantiate(editOptionsTileAttributePrefab, new Vector2(0, 0), Quaternion.identity, transform);
        var attributeEditOptionsTile = editOptionsTileAttribute.GetComponent<TileAttribute>();
        if (attributeEditOptionsTile)
        {
            attributeEditOptionsTile.PositionInTile();
            attributeEditOptionsTile.SetSprite(attributeSprite);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameBoard.instance.selectedEditOptionsTile == editOptionsTileAttribute.GetComponent<TileAttribute>())
        {
            GameBoard.instance.selectedEditOptionsTile = new TileAttribute();
            spriteRenderer.sprite = myFirstSprite;
        }
        else
        {
            //deselect all the other palette tiles:
            foreach (GameObject child in GameBoard.instance.editOptionsPaletteTiles)
            {
                child.GetComponent<EditOptionsTile>().spriteRenderer.sprite = myFirstSprite;
            }

            GameBoard.instance.selectedEditOptionsTile = editOptionsTileAttribute.GetComponent<TileAttribute>();
            GameBoard.instance.selectedEditOptionsTile.Copy(editOptionsTileAttribute.GetComponent<TileAttribute>());
            spriteRenderer.sprite = mySecondSprite;
        }
    }

    private void PositionEditOptionsTileForPalette()
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
