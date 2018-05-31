using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  in game player input and control of the player entity

public class PlayerController : PlayerEntity
{
    [Header("Player")]
    public float jumpForce;
    public float speed;
    public List<string> pushableTags;


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
    private bool changedThisFrame;

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
        grounded = Grounded();
        game.debug.Log("grouded", grounded.ToString());

        base.Update();

        //  toggle between player and ball
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            game.ChangeEntity();
        }

        //  move if active
        if (game.activePlayerEntity == this)
        {
            Move();
        }
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
            //  .25 seconds elapsed, jump no longer pressed
            else
            {
                jumpPressed = false;
            }
        }

        // horizontal input
        hAxis = Input.GetAxisRaw("Horizontal");
        float xForce = 0;
        
        //  cast collider to count horizontal collisions
        RaycastHit2D[] horizontalHits = new RaycastHit2D[16];
        int hitCount = collider_.Cast(new Vector2(hAxis, 0), horizontalHits, 0.01f);
        horizontalHits = new RaycastHit2D[hitCount];

        if (hitCount > 0)
        {
            //  disregard objects with tags in pushable list
            for (int i = 1; i <= hitCount; i++)
            {
                if (pushableTags.Contains(horizontalHits[i].transform.tag))
                {
                    hitCount = hitCount - 1;
                    break;
                }
            }
        }

        // add move force if moving and not hitting a wall
        if (hAxis != 0 && hitCount == 0)
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