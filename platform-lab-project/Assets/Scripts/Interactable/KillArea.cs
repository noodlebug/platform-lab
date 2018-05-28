using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillArea : MonoBehaviour
{
	//	if player enters area, kill player
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();
			player.game.KillPlayer();
		}
	}
}
