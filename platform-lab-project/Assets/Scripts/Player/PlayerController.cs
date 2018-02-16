using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  in game player input and control of the player entity

public class PlayerController : PlayerEntity
{
    public bool enableGravity;
    //  physics script

    [Header("Classic Physics")]  
    //  for normal movement, set all to 1  
    public ClassicPhysics.Modifiers physicsModifiers;

    private ClassicPhysics classicPhysics;


    [Header("Ball")]
    public BallController ballPrefab;
    public BallController ball;

    [Header("Small balls")]
    public SmallBall smallBallPrefab;

    private List<SmallBall> smallBalls = new List<SmallBall>();

    protected override void Awake()
    {
        base.Awake();
        classicPhysics = new ClassicPhysics(this.gameObject, physicsModifiers);
    }

    //  set player velocity from other scripts
    public void SetVelocity(Vector2 velocity)
    {
        classicPhysics.velocity = velocity;
    }

    //  get mouse position
    public Vector2 CursorPosition()
    {
        return game.cam.ScreenToWorldPoint(Input.mousePosition);
    }

    protected override void Update()
    {
        classicPhysics._Update();

        base.Update();
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            BecomeBall();
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnBalls();
        }
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