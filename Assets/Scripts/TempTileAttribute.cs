using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTileAttribute : MonoBehaviour
{

    GameObject thisTempTile;
    RectTransform thisTempRectTransform;
    SpriteRenderer thisTempRenderer;

    void Awake()
    {
        thisTempTile = GetComponent<GameObject>();
        thisTempRectTransform = GetComponent<RectTransform>();
        thisTempRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PositionInTile()
    {
        transform.localScale = new Vector3(100,100,0);
        transform.localPosition = new Vector2(0,0);
    }

    public void RandomColour()
    {
        thisTempRenderer.color = new Color(transform.parent.position.y, transform.parent.position.y, transform.parent.position.y, 1f); // Set to opaque black
    }
}
