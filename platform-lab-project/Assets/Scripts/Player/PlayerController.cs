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
        game.debug.Log("Position X:", System.Math.Round(transform.position.x, 2).ToString());//DEBUG

        //  plot a line between either side of the player, just below the player
        float offset = spriteRenderer.bounds.size.x / 2;
        Vector2 leftBelow = new Vector2(belowPlayer.position.x - offset, belowPlayer.position.y);
        Vector2 rightBelow = new Vector2(belowPlayer.position.x + offset, belowPlayer.position.y);

        //  raycast to check if player is grounded
        grounded = Physics2D.Linecast(leftBelow, rightBelow, 1 << LayerMask.NameToLayer("Ground"));
        game.debug.Log("Grounded: ", grounded.ToString());//DEBUG

        //  on ground and jump pressed
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            jump = true;
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
            Debug.Log("jumping " + rigidBody.ToString());
            rigidBody.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

        //  move
        Move(grounded);
    }

    private void Move(bool grounded = true)
    {
        float hAxis = Input.GetAxis("Horizontal");
        if (hAxis == 0)
        {
            return;            
        }

        //  if not exceeding max speed, accelerate
        if (hAxis * rigidBody.velocity.x < maxSpeed)
        {
            //  airbourne acceleration
            if (!grounded)
            {
                rigidBody.AddForce(Vector2.right * hAxis * airbourneAcceleration);
            }
            //  normal acceleration
            else
            {
                rigidBody.AddForce(Vector2.right * hAxis * acceleration);
            }

            //  clamp speed
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