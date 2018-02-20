using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//	classic platformer player movement

public class ClassicPhysics
{
	public Modifiers modifiers;
	public GameObject mover;
	private Rigidbody2D rigidBody;
	public Vector2 velocity;

	//	player movement input
	private Vector2 horizontalMovement;

	//	collision
	//	buffer radius to prevent overlapping
	const float shell = 0.01f;

	//	custom filter for collisions
	private ContactFilter2D contactFilter;

	//	player is on ground
	private bool grounded;

	// angle of current slope
	private Vector2 groundNormal;

	//	min angle to be considered ground
	
	public ClassicPhysics(GameObject _mover, Modifiers _modifiers)
	{
		mover = _mover;
		modifiers = _modifiers;

		//	do not detect trigger colliders, use player object collision settings
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(mover.layer));

		//	get rigidbody
		rigidBody = mover.GetComponent<Rigidbody2D>();
	}

	public void Input(float hAxis, bool jumpPressed = false, bool jumpReleased = false)
	{	
		//	get horizontal movement input
		velocity.x = (hAxis *2) * modifiers.speed;
		
		//	jump
		if (jumpPressed && grounded)
		{
			velocity.y = 7 * modifiers.jump;
		}
		//	cancel jump
		else if(jumpReleased && velocity.y > 0)
		{
			velocity.y = velocity.y * 0.5f;
		}
	}

	public void Physics()
	{
		//	always set grounded to false
		grounded = false;

		//	apply Unity gravity effect on velocity per frame
		velocity += modifiers.gravity * Physics2D.gravity * Time.deltaTime;			

		//	per second velocity to per frame velocity
		Vector2 positionChange = velocity * Time.deltaTime;

		//	adjust direction to angle of slope
		Vector2 moveHorizontal = new Vector2(groundNormal.y, -groundNormal.x);

		//	move horizontal and vertical
		rigidBody.position = CheckCollision(moveHorizontal * positionChange.x, false);
		rigidBody.position = CheckCollision(Vector2.up * positionChange.y, true);
	}

	private Vector3 CheckCollision(Vector2 offset, bool jumping)
	{
		float distance = offset.magnitude;

		//	if moving check collisions
		if (distance != 0)
		{
			//	cast player collider in direction of movement - get next frame's collisions
			RaycastHit2D[] hits = new RaycastHit2D[16];
			int count = rigidBody.Cast(offset, contactFilter, hits, distance + shell);

			//	trim hits array to list
			List<RaycastHit2D> hitList = new List<RaycastHit2D>();
			for (int i = 0; i < count; i++)
			{
				hitList.Add(hits[i]);
			}



			//	iterate over hits (collisions next frame)
			foreach (RaycastHit2D hit in hitList)
			{
				//	copy hit normal (angle of face)
				Vector2 currentNormal = hit.normal;

				//	check how steep slope is
				if (currentNormal.y > 0.64f * modifiers.minGroundNormalY)
				{
					//	player is on ground
					grounded = true;

					//	if calculating vertical movement, ignore x axis and set current slope angle 
					if (jumping)
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

				//	use cast distance to avoid overlap if not overlapping already
				if (hit.distance > 0)
				{
					distance = Mathf.Min(hit.distance - shell, distance);									
				}
			}
		}

		//	apply movement change
		return rigidBody.position + offset.normalized * distance;
	}

	public void Crouch(float boundsY)
    {
		//	1/2 Y scale
        Vector3 scale = new Vector3(mover.transform.localScale.x, mover.transform.localScale.y / 2, mover.transform.localScale.z);
        mover.transform.localScale = scale;

		//	move down 1/4 height		
		mover.transform.position = mover.transform.position + new Vector3(0, -(boundsY / 4), 0);
    }
	public void Uncrouch(float boundsY)
	{
		// *2 Y scale
		Vector3 scale = new Vector3(mover.transform.localScale.x, mover.transform.localScale.y * 2, mover.transform.localScale.z);
        mover.transform.localScale = scale;

		//	move up 1/4 height
		mover.transform.position = mover.transform.position + new Vector3(0, (boundsY / 2), 0);
	}

	//	used to modify everything above
	//	default values should be 1
	[System.Serializable]
	public struct Modifiers
	{
		public float minGroundNormalY;
		public float acceleration;
		public float speed;
		public float jump;
		public float airbourneAcceleration;
		public float gravity;
	}
}

