using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  the root of the game

public class GameManager : MonoBehaviour
{
    public PlayerController player;

    //  debug
    public DebugThings debug;
    public Transform debugPanel;

    private void Awake()
    {
        player.game = this;
        debug = new DebugThings(debugPanel, debugPanel.GetComponentInChildren<Text>());
    }

    private void Update()
    {
        //  push Update() to non Monobehaviour class DebugThings
        debug.RefreshStrings();
    }
}