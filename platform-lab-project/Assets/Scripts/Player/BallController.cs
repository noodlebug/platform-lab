using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : PlayerEntity
{
	protected override void Awake()
	{
		base.Awake();
	}
	protected override void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			BecomePlayer();
		}
		base.Update();
	}

	//	become normal player entity
	public void BecomePlayer()
	{
		//	correct ball rotation as player is a child of the ball transform
		transform.rotation = new Quaternion();

		//	velocity must be set in ClassicPhysics not RigidBody2D
		game.player.SetVelocity(rigidBody.velocity);
		
		//	activate player
        game.player.gameObject.SetActive(true);

		//	detatch player from ball and destroy ball
		game.player.transform.parent = null;
        Destroy(this.gameObject);
	}
}
