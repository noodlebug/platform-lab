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

    [Header("Ball")]
    public GameObject ballPrefab;
    public BallController ball;

    [Header("Physics modifiers")]
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

    public void SetVelocity(Vector2 velocity)
    {
        classicPhysics.velocity = velocity;
    }

    private void Update()
    {
        //  push Update() to asigned type
        classicPhysics._Update();

        InteractInput();

        if (Input.GetKeyDown(KeyCode.F))
        {
            BecomBall();
        }
    }

    private void FixedUpdate()
    {
        //  push FixedUpdate() to asigned type
        classicPhysics._FixedUpdate();
    }

    private void BecomBall()
    {
        ball = Instantiate(ballPrefab).GetComponent<BallController>();
        ball.transform.position = transform.position;

		//	velocity must be taken from ClassicPhysics not RigidBody2D        
        ball.rigidBody.velocity = classicPhysics.velocity;

        transform.parent = ball.transform;

        ball.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}