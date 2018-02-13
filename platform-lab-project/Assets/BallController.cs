using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : PlayerEntity
{
	private void Awake()
	{
		GetComps();
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			BecomePlayer();
		}
		InteractInput();
	}
	public void BecomePlayer()
	{
		transform.rotation = new Quaternion();
		game.player.transform.parent = null;

		//	velocity must be set in ClassicPhysics not RigidBody2D
		game.player.SetVelocity(rigidBody.velocity);
		
        game.player.gameObject.SetActive(true);
        Destroy(this.gameObject);
	}
}
