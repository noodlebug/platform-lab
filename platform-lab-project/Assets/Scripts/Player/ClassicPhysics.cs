using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicPhysics : PlayerType
{
	private RigidbodyType2D previousBodyType;
	public ClassicPhysics(PlayerController _player) : base(_player)
	{

	}

	public override void Enter()
	{
		previousBodyType = player.rigidBody.bodyType;
		player.rigidBody.bodyType = RigidbodyType2D.Kinematic;
	}

	public override void Exit()
	{
		player.rigidBody.bodyType = previousBodyType;		
	}
}
