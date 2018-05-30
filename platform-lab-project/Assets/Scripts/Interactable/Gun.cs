using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	private GameManager game;
	private GameObject pivot;
	private float timer;

	public Projectile projectilePrefab;
	public float fireRate;

	private void Awake()
	{
		game = GameObject.FindObjectOfType<GameManager>();

		// barrel pivot GameObject
		pivot = transform.GetChild(0).gameObject;

		//	initialise timer
		timer = Time.fixedTime;		
	}

	private void Update()
	{
		//	rotate to quaternion facing player
		pivot.transform.rotation = MyAPI.FromToRotation(game.player.transform.position, this.transform.position);
		
		// fire
		if (Time.fixedTime - timer >= fireRate)
		{
			//	reset timer
			timer = Time.fixedTime;

			//	create projectile aligned with barrel
			Projectile projectileInstance = Instantiate(projectilePrefab, pivot.transform);
			projectileInstance.game = game;
			projectileInstance.transform.SetParent(null);
		}
	}
}
