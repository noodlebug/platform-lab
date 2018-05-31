using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public GameManager game;
	public float speed;
	public float expireTime;

	private float timer;

	private void Awake()
	{
		timer = Time.fixedTime;
	}

	private void Update ()
	{
		// move projectile
		transform.Translate(Vector2.up * speed);

		// remove after expireTime in seconds
		if ((Time.fixedTime - timer) >= expireTime)
		{
			Remove();
		}
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		switch (collider.tag)
        {
            case "Player":
				//game.KillPlayer();
				Remove();
                break;
            case "BigBall":
                Ricochet();
                break;
			default:
				Remove();
				break;
        }
	}

	private void Ricochet()
	{
		//	raycast from behind the projectile
		RaycastHit2D hit = Physics2D.Raycast(transform.position - transform.up, transform.up);

		// calculate angle of ricochet
		Vector2 reflection = Vector2.Reflect((Vector2)transform.up, hit.normal);

		// apply ricochet angle
		transform.rotation = MyAPI.FromToRotation((Vector2)transform.position + reflection, transform.position);
	}

	private void Remove()
	{
		Destroy(gameObject);
	}
}
