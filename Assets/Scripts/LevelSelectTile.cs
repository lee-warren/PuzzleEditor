using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectTile : MonoBehaviour, IPointerClickHandler
{
    GameObject thisTile;
    public SpriteRenderer spriteRenderer;
    RectTransform thisRectTransform;

    public Sprite plusImage;

    public Text levelNumberText;

    public Vector2 position;
    public string levelName;
    bool isNewLevel = false;

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

    public void InitialiseForLevels(int index, string thisLevelName)
    {
        position = new Vector2(index%5, (index/5));
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        levelName = thisLevelName;
        levelNumberText.text = "" + (index + 1);


        PositionTileForLevelSelector();
    }

    public void InitialiseForAddLevelButton(int index, string thisLevelNamePrefix)
    {
        position = new Vector2(index % 5, (index / 5));
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        levelName = thisLevelNamePrefix + "/" + (index + 1) + ".dat";
        isNewLevel = true;

        PositionTileForLevelSelector();

        spriteRenderer.sprite = plusImage;
    }
    

    private void PositionTileForLevelSelector()
    {//sketchy AF... be carefull changing anything in here

        RectTransform parentsRect = thisRectTransform.parent.GetComponent<RectTransform>();
        Vector2 bottomLeftCorner = new Vector2(-parentsRect.sizeDelta.x / 2f, -parentsRect.sizeDelta.x / 2f);

        //sets size of the square relevant to the parents size
        transform.localScale = new Vector3((1 - 1 * spacing) / (float)(5) - spacing, (1 - 1 * spacing) / (float)(3) - spacing, 0);

        transform.localPosition = new Vector2(bottomLeftCorner.x + parentsRect.sizeDelta.x * (spacing + transform.localScale.x / 2f) + position.x * parentsRect.sizeDelta.x * (spacing + transform.localScale.x), bottomLeftCorner.y + parentsRect.sizeDelta.x * (spacing + transform.localScale.y / 2f) + (2 - position.y) * parentsRect.sizeDelta.x * (spacing + transform.localScale.y));
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        if (isNewLevel)
        {
            File.CreateText(levelName).Close();
        }
        LevelSelector.instance.GoToLevel(levelName);
    }
}
