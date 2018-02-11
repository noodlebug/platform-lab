using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlight
{
    public GameObject highlight;
    public GameObject highlightedObject;
    private Camera camera;

    public Bounds bounds;
    private RectTransform rectTransform;
	
	private Vector2 previousPosition;
	private Vector2 previousSize;
	

    //  constructor
    public Highlight(GameObject _highlight, GameObject _highlightedObject, Camera _camera)
    {
		highlight = _highlight;
        highlightedObject = _highlightedObject;		
        camera = _camera;

        bounds = highlightedObject.GetComponent<SpriteRenderer>().bounds;
        rectTransform = highlight.GetComponent<RectTransform>();
    }
    
    //  size of bounds in screen space
    private Vector2 GetScreenSize(Bounds bounds)
    {
        Vector2 topright = camera.WorldToScreenPoint(new Vector2(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y));
        Vector2 bottomleft = camera.WorldToScreenPoint(new Vector2(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y));
        return topright - bottomleft;
    }

    //  match  rect to image size in screen space
    public void UpdateSize()
    {
		//	no change
		if (previousSize == rectTransform.sizeDelta)
		{
			return;
		}

		previousSize = rectTransform.sizeDelta;

        Vector2 size = GetScreenSize(bounds);
		size.x += 10;
		size.y += 10;
        rectTransform.sizeDelta = size;    
    }

    //  set  position to ship position in screen space
    public void UpdatePosition()
    {
		//	no change
		if (previousPosition == (Vector2) rectTransform.position)
		{
			return;
		}

		previousPosition = rectTransform.position;

        highlight.transform.localPosition = (Vector2)camera.WorldToScreenPoint(highlightedObject.transform.position) - new Vector2(Screen.width / 2, Screen.height / 2);
        highlight.transform.rotation = highlightedObject.transform.rotation;
    }

    //  change  sprite
    public void SetSprite(Sprite sprite)
    {
        foreach (Transform child in highlight.transform)
        {
            child.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
            child.localRotation = new Quaternion();
        }
    }

    //  change  color
    public void SetColor(Color color)
    {
        foreach (Transform child in highlight.transform)
        {
            child.GetComponent<Image>().color = color;
        }
    }
}