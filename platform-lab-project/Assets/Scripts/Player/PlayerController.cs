using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  in game player input and control of the player entity
public enum PlayerType {Simulated}; 

public class PlayerController : MonoBehaviour
{
    //  private
    [HideInInspector]public GameManager game;    
    [HideInInspector]public Rigidbody2D rigidBody;
    [HideInInspector]public SpriteRenderer spriteRenderer;
    private List<Interactable> interactables = new List<Interactable>();
    private SimulatedPhysics simulatedPhysics;

    public PlayerType playerType;

    [Header("SimulatedPhysics")]
    public Transform belowPlayer;

    [Header("General physics modifiers")]
    //  delta values applied to all movement scripts
    public float acceleration = 1f;
    public float speed = 1f;
    public float jump = 1f;
    public float airbourneAcceleration = 1f;



    //  when player spawns
    private void Awake()
    {
        rigidBody= GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        simulatedPhysics = new SimulatedPhysics(this, belowPlayer, 300f, 2f, 350f, 50f);
    }
    
    //  //
    #region Interact
            //  //

    //  interactable in range
    public void ExitRange(Interactable interactable)
    {
        interactables.Remove(interactable);
    }

    //  interactable out of range
    public void EnterRange(Interactable interactable)
    {
        interactables.Add(interactable);
    }

    //  interaction things that happen on Update()
    private void InteractUpdateInput()
    {
        game.debug.Log("interactables: ", interactables.Count.ToString());//DEBUG
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (Interactable interactable in interactables)
            {
                interactable.Interact();
            }
        }
    }

    #endregion

    //  //
    #region Update()
            //  //
    private void Update()
    {
        if (playerType == PlayerType.Simulated)
        {
            simulatedPhysics._Update();
        }
        InteractUpdateInput();            
    }

    //  can player jump and is jump pressed
   
    #endregion

    //  //
    #region FixedUpdate()
            //  //

    private void FixedUpdate()
    {
        if (playerType == PlayerType.Simulated)
        {
            simulatedPhysics._FixedUpdate();        
        }
    }

    #endregion
}