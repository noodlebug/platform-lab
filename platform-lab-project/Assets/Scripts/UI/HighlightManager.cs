using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightManager
{
    private UIManager ui;

    //  a game object with 4 child object sprites stuck to the corners
    private GameObject prefab;

    //  all highlights by GameObject
    private Dictionary<GameObject, Highlight> highlights = new Dictionary<GameObject, Highlight>();
    
    public HighlightManager(UIManager _ui, GameObject _prefab)
    {
        ui = _ui;
        prefab = _prefab;
    }

    //  create highlight object
    public void AddHighlight(GameObject highlighted)
    {
        GameObject highlightObject = GameObject.Instantiate(prefab, ui.canvas.transform);
        highlights[highlighted] = new Highlight(highlightObject, highlighted, ui.cam);
    }

    //  destroy highlight object
    public void RemoveHighlight(GameObject unHighlighted)
    {
        GameObject.Destroy(highlights[unHighlighted].highlight);

        highlights.Remove(unHighlighted);
    }

    //  update all highlight object sizes and positions
    public void UpdateHighlights()
    {
        foreach (KeyValuePair<GameObject, Highlight> kvp in highlights)
        {
            kvp.Value.UpdatePosition();
            kvp.Value.UpdateSize();            
        }
    }

    //  set highlight color
    public void SetColor(GameObject highlighted, Color color)
    {
        if (highlights.ContainsKey(highlighted))
        {
            highlights[highlighted].SetColor(color);
        }
    }
}
