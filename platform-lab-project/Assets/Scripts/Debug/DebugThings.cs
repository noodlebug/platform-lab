﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  custom debugging things

public class DebugThings
{
    private Transform panel;
    private Text text;
    public Dictionary<string, string> logs = new Dictionary<string, string>();
    public Dictionary<string, Vector2[]> lines = new Dictionary<string, Vector2[]>();

    //  constructor
    public DebugThings(Transform _panel, Text _text)
    {
        panel = _panel;
        text = _text;
        text.text = "";
        panel.GetComponent<Button>().onClick.AddListener(delegate { Toggle(); } );
    }

    //  set active to !isActive
    private void Toggle()
    {
        text.gameObject.SetActive(!text.gameObject.activeInHierarchy);
    }

    //  add given values to dict of strings
    public void Log(string key, string value)
    {
        logs[key] = value;
    }

    //  add vectors for gizmo lines
    public void Line(string key, Vector2 start, Vector2 end)
    {
        lines[key] = new Vector2[2] { start, end };
    }

    //  print dict of strings as one string in UI Text component
    public void Refresh()
    {
        text.text = "";
        foreach (KeyValuePair<string, string> kvp in logs)
        {
            text.text += kvp.Key + ":\t" + kvp.Value + "\n";
        }
    }

    
}