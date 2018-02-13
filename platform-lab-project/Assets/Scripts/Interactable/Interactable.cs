using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//	things that the player can interact with

public class Interactable : MonoBehaviour
{
	//	can be assigned to a function in a game object in inspector
	public UnityEvent interact = new UnityEvent();

	//	linked buttons are always usable at the same time
	public List<Interactable> linked = new List<Interactable>();

	private void Awake()
	{	
		linked.Add(this);					
		//	link linked objects
		if (linked.Count > 1)
		{
			foreach (Interactable linkedObject in linked)
			{
				linkedObject.Link(this);
			}
		}	
	}

	public float XPos()
	{
		return transform.position.x;
	}
	//	enable custom interaction in inhereting classes
	public virtual void Interact()
	{
		interact.Invoke();
	}

	//	collides with something
	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerEntity entity = collision.gameObject.GetComponent<PlayerEntity>();
		if (entity != null && entity.canInteract)
		{
			entity.game.ui.interactable.EnterRange(this);
		}
	}

	//	exits collision
	private void OnTriggerExit2D(Collider2D collision)
	{
		PlayerEntity entity = collision.gameObject.GetComponent<PlayerEntity>();
		if (entity != null && entity.canInteract)
		{
			entity.game.ui.interactable.ExitRange(this);
		}
	}

	//	link all interactables in the caller's list + caller
	public void Link(Interactable interactable)
	{
		// add list
		foreach (Interactable linkedObject in interactable.linked)
		{
			if (!linked.Contains(linkedObject))
			{
				linked.Add(linkedObject);
			}
		}
	}
}
