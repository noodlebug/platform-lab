using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    [HideInInspector]public GameManager game;    
    [HideInInspector]public Rigidbody2D rigidBody;
    [HideInInspector]public SpriteRenderer spriteRenderer;

	public bool canInteract;

	protected void GetComps()
    {
		game = GameObject.FindObjectOfType<GameManager>();
        rigidBody= GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected void InteractInput()
    {
		if (!canInteract)
		{
			return;
		}
        if (Input.GetKeyDown(KeyCode.E))
        {
            game.ui.interactable.Interact();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            game.ui.interactable.SelectNext();
        }
    }
}
