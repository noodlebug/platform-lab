using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    [HideInInspector]public GameManager game;    
    [HideInInspector]public Rigidbody2D rigidBody;
    [HideInInspector]public SpriteRenderer spriteRenderer;

	public bool canInteract;
    public bool isPlayer;

    //  run in awake
	protected virtual void Awake()
    {
		game = GameObject.FindObjectOfType<GameManager>();
        rigidBody= GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

    protected virtual void Update()
	{
		InteractInput();
	}

	protected void InteractInput()
    {
		if (!canInteract || !isPlayer)
		{
			return;
		}        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(name);            
            game.ui.interactable.Interact();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            game.ui.interactable.SelectNext();
        }
    }
}
