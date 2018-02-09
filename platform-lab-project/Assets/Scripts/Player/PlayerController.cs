using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  in game player input and control of the player entity

public class PlayerController : MonoBehaviour
{
    //  private
    private Rigidbody2D rigidBody;
    private bool move;
    private bool jump = false;
    private bool grounded = false;
    private Vector2 normalScale;

    private SpriteRenderer spriteRenderer;

    //  public
    public GameManager game;
    public Transform belowPlayer;
    public float acceleration = 300f;
    public float maxSpeed = 5f;
    public float jumpForce = 250f;
    public float airbourneAcceleration = 150f;

    //  when player spawns
    private void Awake()
    {
        rigidBody= GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalScale = transform.localScale;
    }
    
    //  every frame
    private void Update()
    {
        //  //  //  //  //  //  DEBUG

        game.debug.Log("Position X:", System.Math.Round(transform.position.x, 2).ToString());

        //  //  //  //  //  //

        //  offset is half the width of the player sprite
        float offset = spriteRenderer.bounds.size.x / 2;

        //  get two points, below the player object and at either edge of it's horizontal bounds
        Vector2 leftBelow = new Vector2(belowPlayer.position.x - offset, belowPlayer.position.y);
        Vector2 rightBelow = new Vector2(belowPlayer.position.x + offset, belowPlayer.position.y);

        //  cast a 2D ray from left to right, below the player
        //  https://forum.unity.com/threads/c-help-1-layermask-nametolayer-environment.224932/ 1 << LayerMask.NameToLayer("Ground")
        grounded = Physics2D.Linecast(leftBelow, rightBelow, 1 << LayerMask.NameToLayer("Ground"));

        //  Input.GetKeyDown must be checked in Update() (every frame)
        //  because it will only be true for the frame where the key is pressed
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            jump = true;
            //  jump is now true until the next FixedUpdate() runs
            //  at which point Jump() runs and jump is set to false
        }

        //  flip sprite when direction keys are pressed
        if (Input.GetKeyDown(KeyCode.D))
        {
            FaceRight();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            FaceLeft();
        }

        //  fix rotation to prevent obeject from falling over
        transform.rotation = new Quaternion();
    }
    
    //  50 times per second
    private void FixedUpdate()
    {
        //  jump
        if (grounded && jump)
        {
            rigidBody.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

        //  move
        Move(grounded);
    }

    private void Move(bool grounded = true)
    {
        //  GetAxis() has A assigned to negative and D assigned to positive
        //  and works well with controller joysticks
        float hAxis = Input.GetAxis("Horizontal");
        
        //  returns 0 if nothing pressed
        if (hAxis == 0)
        {
            return;
        }

        //  if not exceeding max speed, accelerate
        if (hAxis * rigidBody.velocity.x < maxSpeed)
        {
            //  less acceleration if airbourne
            if (!grounded)
            {
                rigidBody.AddForce(Vector2.right * hAxis * airbourneAcceleration);
            }
            //  normal acceleration
            else
            {
                rigidBody.AddForce(Vector2.right * hAxis * acceleration);
            }

            //  if accelerated past max speed, clamp speed
            if (Mathf.Abs(rigidBody.velocity.x) > maxSpeed)
            {
                rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * maxSpeed, rigidBody.velocity.y);
            }
        } 
    }

    //turn left or right
    private void FaceRight()
    {
        Turn(1);
    }
    private void FaceLeft()
    {
        Turn(-1);
    }
    private void Turn(int sign)
    {
        //  if on ground stop moving before changing movement direction
        if (grounded)
        {
            rigidBody.velocity = Vector2.zero;
        }
        Vector2 scale = transform.localScale;
        scale.x = sign;
        transform.localScale = scale;
    }
}