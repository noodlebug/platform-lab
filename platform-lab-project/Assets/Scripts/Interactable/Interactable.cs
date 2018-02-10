using UnityEngine;
using UnityEngine.Events;

//	things that the player can interact with

public class Interactable : MonoBehaviour
{
	//	can be assigned to a function in a game object in inspector
	public UnityEvent interact = new UnityEvent();

	//	enable custom interaction in inhereting classes
	public virtual void Interact()
	{
		interact.Invoke();
	}

	//	collides with something
	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerController player = collision.gameObject.GetComponent<PlayerController>();
		if (player != null)
		{
			player.EnterRange(this);
		}
	}

	//	exits collision
	private void OnTriggerExit2D(Collider2D collision)
	{
		PlayerController player = collision.gameObject.GetComponent<PlayerController>();
		if (player != null)
		{
			player.ExitRange(this);
		}
	}
}
