using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  in game player input and control of the player entity

public class PlayerController : PlayerEntity
{
    [Header("Player")]
    public float jumpForce;
    public float speed;
    

    [Header("Ball")]
    public BallController ballPrefab;
    public BallController ball;

    [Header("Small balls")]
    public SmallBall smallBallPrefab;
    private List<SmallBall> smallBalls = new List<SmallBall>();

    [Header("Minions")]
    public Minion minionPrefab;
    private List<Minion> minions = new List<Minion>();

    // movement
    private float jumpPressedTime = 0;
    private bool jumpPressed = false;
    private float hAxis;

    protected override void Awake()
    {
        base.Awake();
    }

    //  set player velocity from other scripts
    public void 
    SetVelocity(Vector2 velocity)
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
        grounded = Grounded();

        game.debug.Log("grouded", grounded.ToString());

        base.Update();

        Move();

        ToyInput();
    }

    private void FixedUpdate()
    {
    }

    #region Movement

    private void Move()
    {
        // store time that jump was pressed
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressedTime = Time.time;
            jumpPressed = true;
        }
        //stop jumping (half y velocity if moving up)
        else if (Input.GetButtonUp("Jump") && rigidBody.velocity.y > 0)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y / 2);
        }

        //  enable player to press jump a moment before landing
        if (jumpPressed)
        {   
            //  jump was pressed within 0.25f seconds of current time
            if ((Time.time - jumpPressedTime < 0.25f))
            {
                //  wait until grounded to jump
                if (grounded)
                {
                    rigidBody.AddForce(new Vector2(0, jumpForce));
                    jumpPressed = false;
                }
            }
            //  2.5 seconds elapsed, jump no longer pressed
            else
            {
                jumpPressed = false;
            }
        }

        // horizontal input
        hAxis = Input.GetAxisRaw("Horizontal");
        
        float xForce = 0;

        // move force
        if (hAxis != 0 && (Input.GetButtonDown("Horizontal") || (rigidBody.velocity.x != 0 || grounded)))
        {
            xForce = (rigidBody.mass * (speed - (rigidBody.velocity.x * hAxis))) * hAxis;
        }
        // stop force
        else if (rigidBody.velocity.x != 0)
        {
            xForce = (rigidBody.mass * rigidBody.velocity.x) * -1;
        }

        // add horizontal force
        rigidBody.AddForce(new Vector2(xForce, 0), mode: ForceMode2D.Impulse);

        // if moving and facing the wrong direction
        if (hAxis != 0 && facingRight != (hAxis > 0))
        {
            //  set current direction
            facingRight = (hAxis > 0);
            //  change sprite
            spriteRenderer.sprite = SetSprite();
        }
    }

    #endregion

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

        if (Input.GetKeyDown(KeyCode.LeftControl))
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