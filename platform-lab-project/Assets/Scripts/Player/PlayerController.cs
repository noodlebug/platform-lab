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

    private SpriteRenderer spriteRenderer;

    //  public
    public Transform belowPlayer;
    public float acceleration = 300f;
    public float maxSpeed = 5f;
    public float jumpForce = 250f;

    //  when player spawns
    private void Awake()
    {
        rigidBody= GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    //  every frame
    private void Update()
    {
        //  offset is half the width of the player sprite
        float offset = spriteRenderer.bounds.size.x / 2;

        //  get two points, below the player object and at either edge of it's horizontal bounds
        Vector2 leftBelow = new Vector2(belowPlayer.position.x - offset, belowPlayer.position.y);
        Vector2 rightBelow = new Vector2(belowPlayer.position.x + offset, belowPlayer.position.y);

        //  cast a 2D ray from the center of the player object to an object below it
        //  Physics2D.Linecast() returns true if it hits object in "ground" layer (Layers are assigned in Unity)
        //  https://forum.unity.com/threads/c-help-1-layermask-nametolayer-environment.224932/ 1 << LayerMask.NameToLayer("Ground")
        grounded = Physics2D.Linecast(leftBelow, rightBelow, 1 << LayerMask.NameToLayer("Ground"));

        //  Input.GetKeyDown must be checked in Update() (every frame)
        //  Sbecause it will only be true for the frame where the key is pressed
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
        if (grounded)
        {
            //  GetAxis() has A assigned to negative and D assigned to positive
            //  and works well with controller joysticks
            float hAxis = Input.GetAxis("Horizontal");

            //  if not exceeding max speed, accelerate
            if (hAxis * rigidBody.velocity.x < maxSpeed)
            {
                rigidBody.AddForce(Vector2.right * hAxis * acceleration);
            }

            //  if accelerated past max speed, clamp speed
            if (Mathf.Abs(rigidBody.velocity.x) > maxSpeed)
            {
                rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * maxSpeed, rigidBody.velocity.y);
            }

            //  jump
            if (jump)
            {
                rigidBody.AddForce(new Vector2(0f, jumpForce));
                jump = false;
            }
        }
    }
    
    //  flip sprite by setting it's transform x scale to 1 or -1
    private void FaceRight()
    {
        Vector2 scale = transform.localScale;
        scale.x = 1;
        transform.localScale = scale;
    }
    private void FaceLeft()
    {
        Vector2 scale = transform.localScale;
        scale.x = -1;
        transform.localScale = scale;
    }
}
