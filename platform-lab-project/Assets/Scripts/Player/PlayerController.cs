using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  in game player input and control of the player entity

public class PlayerController : PlayerEntity
{
    public bool enableGravity;
    //  physics script
    private ClassicPhysics classicPhysics;

    [Header("Classic Physics")]
    float minGroundNormalY = 0.64f;

    [Header("Ball")]
    public BallController ballPrefab;
    public BallController ball;

    [Header("Small balls")]
    public SmallBall smallBallPrefab;

    private List<SmallBall> smallBalls = new List<SmallBall>();

    [Header("Physics modifiers")]
    public float acceleration = 1f;
    public float speed = 1f;
    public float jump = 1f;
    public float airbourneAcceleration = 1f;
    public float gravity = 1f;

    //  when player spawns
    protected override void Awake()
    {
        base.Awake();
        classicPhysics = new ClassicPhysics(this, minGroundNormalY);
    }

    public void SetVelocity(Vector2 velocity)
    {
        classicPhysics.velocity = velocity;
    }

    public Vector2 CursorPosition()
    {
        return game.cam.ScreenToWorldPoint(Input.mousePosition);
    }

    protected override void Update()
    {
        classicPhysics._Update();

        if (Input.GetKeyDown(KeyCode.F))
        {
            BecomeBall();
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnBalls();
        }


        base.Update();
    }

    private void FixedUpdate()
    {
        classicPhysics._FixedUpdate();
    }

    //  turn player into a ball
    private void BecomeBall()
    {
        //  set ball position to player position
        ball = Instantiate(ballPrefab);
        ball.transform.position = transform.position;

		//	player velocity must be taken from ClassicPhysics not RigidBody2D        
        ball.rigidBody.velocity = classicPhysics.velocity;

        //  attach player to ball
        transform.parent = ball.transform;

        //  activate ball deactivate player
        ball.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    //  spawn some small balls
    private void SpawnBalls()
    {
        for (int i = 0; i < 10; i++)
        {
            //  create a ball
            SmallBall smBall = Instantiate(smallBallPrefab);

            //  place the ball near the player
            smBall.transform.position = transform.position + new Vector3(Random.Range(0.05f, 0.5f), Random.Range(0.05f, 0.5f));

            smBall.player = this;

            //  track balls
            smallBalls.Add(smBall);
        }
    }
}