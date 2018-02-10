using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerType
{
	protected PlayerController player;
	public PlayerType(PlayerController _player)
	{
		player = _player;
	}

	public virtual void Enter() { }
	public virtual void Exit() { }
	public virtual void _Update() { }
	public virtual void _FixedUpdate() { }
}
