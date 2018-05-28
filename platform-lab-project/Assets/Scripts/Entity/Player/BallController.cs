using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : PlayerEntity
{
	public float maxSpeed;

	protected override void Awake()
	{
		base.Awake();
	}
	protected override void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			BecomePlayer();
		}
		
		Move();

		base.Update();
	}

	//	become normal player entity
	public void BecomePlayer()
	{
		//	correct ball rotation as player is a child of the ball transform
		transform.rotation = new Quaternion();

		//	set player velocity to ball velocity
		game.player.SetVelocity(rigidBody.velocity);

		//	activate player
        game.player.gameObject.SetActive(true);

		//	detatch player from ball and destroy ball
		game.player.transform.parent = null;
        Destroy(this.gameObject);
	}

	private void Move()
	{
		// horizontal axis is reversed so the ball rolls in the correct direction
		float hAxis = -Input.GetAxisRaw("Horizontal");

		game.debug.Log("horizontal axis", hAxis.ToString());
		game.debug.Log("angular sign", Mathf.Sign(rigidBody.angularVelocity).ToString());
		game.debug.Log("angular velocity", Mathf.Round(rigidBody.angularVelocity).ToString());

		if (hAxis != 0)
		{
			//	above max speed and not trying to reverse
			if (Mathf.Sign(rigidBody.angularVelocity) == hAxis && Mathf.Abs(rigidBody.angularVelocity) >= maxSpeed)
			{
				return;
			}

			//	apply rotational force
			rigidBody.AddTorque(hAxis, mode: ForceMode2D.Impulse);
			
			//	clamp speed
			if (Mathf.Abs(rigidBody.angularVelocity) > maxSpeed)
			{
				rigidBody.angularVelocity = maxSpeed * Mathf.Sign(rigidBody.angularVelocity);
			}
			
		}
	}
}
