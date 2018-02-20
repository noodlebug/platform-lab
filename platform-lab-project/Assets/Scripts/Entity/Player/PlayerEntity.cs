using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity
{
	public bool canInteract;
    public bool isPlayer;    

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
