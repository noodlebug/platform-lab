using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  in game player input and control of the player entity

public class PlayerController : PlayerEntity
{ 
    //  physics script
    private ClassicPhysics classicPhysics;

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
        GetComps();
        classicPhysics = new ClassicPhysics(this, minGroundNormalY);
    }

    private void Update()
    {
        //  push Update() to asigned type
        classicPhysics._Update();

        InteractInput();            
    }

    private void FixedUpdate()
    {
        //  push FixedUpdate() to asigned type        
        classicPhysics._FixedUpdate();        
    }
}