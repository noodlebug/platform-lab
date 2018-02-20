using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBall : MonoBehaviour
{
	private Rigidbody2D rigidBody;
	public PlayerController player;
	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	private void Update ()
	{
		//	move balls towards cursor
		rigidBody.velocity += ((Vector2)((Vector3)player.CursorPosition() - transform.position).normalized * Time.deltaTime) * 2;
	}
}
