using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedPhysics
{
	//	constructor variables
	private PlayerController player;
    private Transform belowPlayer;
    private float acceleration = 300f;
    private float maxSpeed = 2f;
    private float jumpForce = 350f;
    private float airbourneAcceleration = 50f;
	
	private bool grounded = false;
    private bool jump = false;
	
	//	constructor
	public SimulatedPhysics(PlayerController _player, Transform _belowPlayer, float _acceleration, float _maxSpeed, float _jumpForce, float _airbourneAcceleration)
	{
		player = _player;
		belowPlayer = _belowPlayer;

		//	apply delta values in PlayerController
		acceleration = _acceleration * player.acceleration;
		maxSpeed = _maxSpeed * player.speed;
		jumpForce = _jumpForce * player.jump;
		airbourneAcceleration = _airbourneAcceleration * player.airbourneAcceleration;
	}

	//  //
    #region _Update()
            //  //
	public void _Update()
	{
		player.game.debug.Log("Position X:", System.Math.Round(player.transform.position.x, 2).ToString());//DEBUG

        CheckJump();

        CheckFacing();

        //  fix rotation to prevent obeject from falling over
        player.transform.rotation = new Quaternion();
	}

	private void CheckJump()
    {
        //  plot a line between either side of the player, just below the player
        float offset = player.spriteRenderer.bounds.size.x / 2;
        Vector2 leftBelow = new Vector2(belowPlayer.position.x - offset, belowPlayer.position.y);
        Vector2 rightBelow = new Vector2(belowPlayer.position.x + offset, belowPlayer.position.y);

        //  raycast to check if player is grounded
        grounded = Physics2D.Linecast(leftBelow, rightBelow, 1 << LayerMask.NameToLayer("Ground"));
		player.game.debug.Log("Grounded: ", grounded.ToString());//DEBUG

        //  on ground and jump pressed
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            jump = true;
        }
    }

    //  direction key pressed
    private void CheckFacing()
    {
        //  flip sprite when direction keys are pressed
        if (Input.GetKeyDown(KeyCode.D))
        {
            Turn(1);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Turn(-1);
        }
    }

    //turn left or right depending on sign
    private void Turn(int sign)
    {
        //  if on ground stop moving before changing movement direction
        if (grounded)
        {
            player.rigidBody.velocity = Vector2.zero;
        }
        Vector2 scale = player.transform.localScale;
        scale.x = sign;
        player.transform.localScale = scale;
    }

	#endregion

	//  //
    #region _FixedUpdate()
            //  //

	public void _FixedUpdate()
    {
        Jump();

        Move(grounded);
    }

    //  add upward force
    private void Jump()
    {
        if (grounded && jump)
        {
            player.rigidBody.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }

    //  add force relative to Input.GetAxis() and clamp speed to max
    private void Move(bool grounded = true)
    {
        float hAxis = Input.GetAxis("Horizontal");
        if (hAxis == 0)
        {
            return;            
        }

        //  if not exceeding max speed, accelerate
        if (hAxis * player.rigidBody.velocity.x < maxSpeed)
        {
            //  airbourne acceleration
            if (!grounded)
            {
                player.rigidBody.AddForce(Vector2.right * hAxis * airbourneAcceleration);
            }
            //  normal acceleration
            else
            {
                player.rigidBody.AddForce(Vector2.right * hAxis * acceleration);
            }

            //  clamp speed
            if (Mathf.Abs(player.rigidBody.velocity.x) > maxSpeed)
            {
                player.rigidBody.velocity = new Vector2(Mathf.Sign(player.rigidBody.velocity.x) * maxSpeed, player.rigidBody.velocity.y);
            }
        } 
    }

	#endregion
}