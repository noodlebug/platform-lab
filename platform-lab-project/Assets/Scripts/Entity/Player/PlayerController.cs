using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  in game player input and control of the player entity

public class PlayerController : PlayerEntity
{
    public bool crouched;
    
    //  last time jump was pressed, set to -100 to avoid jumping on play
    private float jumpPressed = -100;

    [Header("Ball")]
    public BallController ballPrefab;
    public BallController ball;

    [Header("Small balls")]
    public SmallBall smallBallPrefab;

    private List<SmallBall> smallBalls = new List<SmallBall>();

    [Header("Minions")]
    public Minion minionPrefab;

    private List<Minion> minions = new List<Minion>();

    protected override void Awake()
    {
        base.Awake();
    }

    //  set player velocity from other scripts
    public void SetVelocity(Vector2 velocity)
    {
        rigidBody.velocity = velocity;
    }

    //  get mouse position
    public Vector2 CursorPosition()
    {
        return game.cam.ScreenToWorldPoint(Input.mousePosition);
    }

    protected override void Update()
    {
        base.Update();

        /*float hAxis = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = Time.time;
        }

        // if moving and facing the wrong direction
        if (hAxis != 0 && facingRight != (hAxis > 0))
        {
            //  set current direction
            facingRight = (hAxis > 0);
            //  change sprite
            spriteRenderer.sprite = GetSprite();
        }
        
        //  movement input
        physics.Input(hAxis, (Time.time - jumpPressed < 0.1f), Input.GetButtonUp("Jump"));

        if (Input.GetKeyDown(KeyCode.S))
        {
            //  crouch down
            physics.Crouch(spriteRenderer.bounds.size.y);
            crouched = true;
        }
        else if (!Input.GetKey(KeyCode.S) && crouched)
        {
            //  crouch up
            physics.Uncrouch(spriteRenderer.bounds.size.y);
            crouched = false;
        }*/

        ToyInput();
    }



    //  //
    #region Toys
            //  //

    private void ToyInput()
    {
        //  command minions to move
        if (Input.GetButtonDown("Fire1"))
        {
            foreach (Minion minion in minions)
            {
                minion.MoveToPoint(CursorPosition());
            }
        } 

        if (Input.GetKeyDown(KeyCode.F))
        {
            BecomeBall();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnBalls();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            //SpawnMinions();
        }
    }

    private void FixedUpdate()
    {
        //physics.Physics();
    }

    //  turn player into a ball
    private void BecomeBall()
    {
        //  set ball position to player position
        ball = Instantiate(ballPrefab);
        ball.transform.position = transform.position;

		//	player velocity must be taken from ClassicPhysics not RigidBody2D        
        ball.rigidBody.velocity = rigidBody.velocity;

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

    //  spawn some minions
    private void SpawnMinions()
    {
        for (int i = 0; i < 1; i++)
        {
            //  create a minion
            Minion minion = Instantiate(minionPrefab);

            //  place the minion near the player
            minion.transform.position = transform.position + new Vector3(Random.Range(0.05f, 0.5f), Random.Range(0.05f, 0.5f));

            minion.player = this;
            minion.ai = new StateMachine(this);

            //  track minion
            minions.Add(minion);
        }
    }

    //  command all minions to move in the direction of a point
    private void MoveMinions(Vector2 point)
    {
        foreach (Minion minion in  minions)
        {
            minion.MoveToPoint(point);
        }
    }

    #endregion
}