using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  in game player input and control of the player entity

public class PlayerController : MonoBehaviour
{
    //  private
    [HideInInspector]public GameManager game;    
    [HideInInspector]public Rigidbody2D rigidBody;
    [HideInInspector]public SpriteRenderer spriteRenderer;
 
    //  player types
    public PlayerType previousType;
    public PlayerType currentType;
    private SimulatedPhysics simulatedPhysics;
    private ClassicPhysics classicPhysics;

    [Header("SimulatedPhysics")]
    public Transform belowPlayer;

    [Header("Classic Physics")]
    float minGroundNormalY = 0.64f;

    [Header("General physics modifiers")]
    //  delta values applied to all movement scripts
    public float acceleration = 1f;
    public float speed = 1f;
    public float jump = 1f;
    public float airbourneAcceleration = 1f;
    public float gravity = 1f;



    //  when player spawns
    private void Awake()
    {
        rigidBody= GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //  instantiate player type classes
        simulatedPhysics = new SimulatedPhysics(this, belowPlayer, 300f, 2f, 350f, 50f);
        classicPhysics = new ClassicPhysics(this, minGroundNormalY);

        classicPhysics.Enter();
        currentType = classicPhysics;
    }

    //  //
    #region Player Types
            //  //
    
    //  set types
    public void SetSimulatedType()
    {
        SetType(simulatedPhysics);
    }

    public void SetClassicType()
    {
        SetType(classicPhysics);
    }

    public void SetType(PlayerType playerType)
    {
        //  exit previous type and store        
        currentType.Exit();
        previousType = currentType;

        //  enter current type and assign
        playerType.Enter();
        currentType = playerType;
    }

    #endregion

    //  //
    #region Interact
            //  //

    //  interaction things that happen on Update()
    private void InteractUpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            game.ui.interactable.Interact();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            game.ui.interactable.SelectNext();
        }
    }

    #endregion

    private void Update()
    {
        //  push Update() to asigned type
        currentType._Update();

        InteractUpdateInput();            
    }

    private void FixedUpdate()
    {
        //  push FixedUpdate() to asigned type        
        currentType._FixedUpdate();        
    }
}