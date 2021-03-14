using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAttribute : MonoBehaviour
{

    GameObject thisAttributeTile;
    public RectTransform thisAttributeRectTransform;
    public SpriteRenderer thisAttributeRenderer;

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
        transform.localScale = new Vector3(100, 100, 0);
        transform.localPosition = new Vector2(0, 0);

        thisAttributeRenderer.color = tileToCopy.thisAttributeRenderer.color;
    }

    public void PositionInTile()
    {
        transform.localScale = new Vector3(100,100,0);
        transform.localPosition = new Vector2(0,0);
    }

    public void RandomColour()
    {
        thisAttributeRenderer.color = new Color(transform.parent.position.y, transform.parent.position.y, transform.parent.position.y, 1f); 
    }
}
