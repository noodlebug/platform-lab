using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//	classic platformer player movement

public class ClassicPhysics
{
	private PlayerController player;	
	//	main direction/speed
	private Vector2 velocity;

	//	player movement input
	private Vector2 horizontalMovement;

	//	collision
	//	buffer radius to prevent overlapping
	const float shell = 0.01f;

	//	custom filter for collisions
	private ContactFilter2D contactFilter;

	//	min angle to be considered ground
	private float minGroundNormalY;

	//	player is on ground
	private bool grounded;

	// angle of current slope
	private Vector2 groundNormal;


	//	correct change to player rigidbody on exit
	private RigidbodyType2D previousBodyType;

	
	public ClassicPhysics(PlayerController _player, float _minGroundNormalY)
	{
		player = _player;
		minGroundNormalY = _minGroundNormalY;

		//	do not detect trigger colliders, use player object collision settings
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(player.gameObject.layer));
		 
	}

	public void _Update()
	{
		ComputeVelocity();
	}

	private void ComputeVelocity()
	{		
		//	get horizontal movement input
		velocity.x = (Input.GetAxis("Horizontal") *2) * player.speed;

		//	jump
		if (Input.GetButtonDown("Jump") && grounded)
		{
			velocity.y = 7 * player.jump;
		}
		//	cancel jump
		else if(Input.GetButtonUp("Jump"))
		{
			if (velocity.y > 0)
			{
				velocity.y = velocity.y * 0.5f;
			}
		}
	}

	public void _FixedUpdate()
	{
		//	always set grounded to false
		grounded = false;

		//	apply Unity gravity effect on velocity per frame
		velocity += player.gravity * Physics2D.gravity * Time.deltaTime;

		//	per second velocity to per frame velocity
		Vector2 positionChange = velocity * Time.deltaTime;

		//	adjust direction to angle of slope
		Vector2 moveHorizontal = new Vector2(groundNormal.y, -groundNormal.x);

		//	move horizontal and vertical
		Movement(moveHorizontal * positionChange.x, false);
		Movement(Vector2.up * positionChange.y, true);
	}

	private void Movement(Vector2 offset, bool yMovement)
	{
		float distance = offset.magnitude;

		//	if moving check collisions
		if (distance != 0)
		{
			//	cast player collider in direction of movement - get next frame's collisions
			RaycastHit2D[] hits = new RaycastHit2D[16];
			int count = player.rigidBody.Cast(offset, contactFilter, hits, distance + shell);

			//	trim hits array to list
			List<RaycastHit2D> hitList = new List<RaycastHit2D>();
			for (int i = 0; i < count; i++)
			{
				hitList.Add(hits[i]);
			}

			player.game.debug.Log("hit count: ", count.ToString());//DEBUG


			//	iterate over hits (collisions next frame)
			foreach (RaycastHit2D hit in hitList)
			{
				//	copy hit normal (angle of face)
				Vector2 currentNormal = hit.normal;

				//	check how steep slope is
				if (currentNormal.y > minGroundNormalY)
				{
					//	player is on ground
					grounded = true;

					//	if calculating vertical movement, ignore x axis and set current slope angle 
					if (yMovement)
					{
						groundNormal = currentNormal;
						currentNormal.x = 0;
					}
				}

				//	prevents angled surface from stopping movement dead
				float projection = Vector2.Dot(velocity, currentNormal);
				if (projection < 0)
				{
					velocity = velocity - projection * currentNormal;
				}

				//	apply our shell radius to the cast distance ensure no overlaps
				float modifiedDistance = hit.distance - shell;
				//	move our intended distance if less than the cast (no overlap), otherwise use the cast distance (to avoid overlap)
				distance = modifiedDistance < distance ? modifiedDistance : distance;
			}
		}

		//	apply movement change
		player.rigidBody.position = player.rigidBody.position + offset.normalized * distance;
	}
}
