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
		if (game.activePlayerEntity == this)
        {
            Move();
        }

		base.Update();
	}

	private void Move()
	{
		// horizontal axis is reversed so the ball rolls in the correct direction
		float hAxis = -Input.GetAxisRaw("Horizontal");

		//	stop
		if (Input.GetKey(KeyCode.S))
		{
			rigidBody.angularVelocity = rigidBody.angularVelocity / 2;
		}

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
