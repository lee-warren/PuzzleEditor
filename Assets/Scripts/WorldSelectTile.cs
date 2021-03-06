using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldSelectTile : MonoBehaviour, IPointerClickHandler
{
    GameObject thisTile;
    public SpriteRenderer spriteRenderer;
    RectTransform thisRectTransform;

    public Sprite plusImage;

    public Vector2 position;
    public string worldName;
    public Text worldNumberText;
    public bool isNewWorld = false;

    private float spacing = 1f / 100f;

    void Awake()
    {
        thisTile = GetComponent<GameObject>();
        thisRectTransform = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitialiseForWorlds(int index, string thisWorldName)
    {
        position = new Vector2(index % 5, (index / 5));
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        worldName = thisWorldName; //(index + 1).ToString();
        worldNumberText.text = worldName;

        PositionTileForWorldSelector();
    }

    public void InitialiseForAddWorldButton(int index)
    {
        position = new Vector2(index % 5, (index / 5));
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        worldName = (index + 1).ToString();

        PositionTileForWorldSelector();

        spriteRenderer.sprite = plusImage;

        isNewWorld = true;
    }


    private void PositionTileForWorldSelector()
    {//sketchy AF... be carefull changing anything in here

        RectTransform parentsRect = thisRectTransform.parent.GetComponent<RectTransform>();
        Vector2 bottomLeftCorner = new Vector2(-parentsRect.sizeDelta.x / 2f, -parentsRect.sizeDelta.x / 2f);

        //sets size of the square relevant to the parents size
        transform.localScale = new Vector3((1 - 1 * spacing) / (float)(5) - spacing, (1 - 1 * spacing) / (float)(3) - spacing, 0);

        transform.localPosition = new Vector2(bottomLeftCorner.x + parentsRect.sizeDelta.x * (spacing + transform.localScale.x / 2f) + position.x * parentsRect.sizeDelta.x * (spacing + transform.localScale.x), bottomLeftCorner.y + parentsRect.sizeDelta.x * (spacing + transform.localScale.y / 2f) + (2 - position.y) * parentsRect.sizeDelta.x * (spacing + transform.localScale.y));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isNewWorld)
        {
            //create the world
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Levels/" + worldName);

        }
        LevelSelector.instance.LoadLevelScreen(worldName);

    }
}
