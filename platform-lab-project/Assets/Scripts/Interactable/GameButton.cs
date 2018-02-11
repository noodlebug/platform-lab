using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//	interactable button in game world

public class GameButton : Interactable
{
	//	sprite renderer component
	public SpriteRenderer sprite;
	//	sprite images
	public Sprite onSprite;
	public Sprite offSprite;

	public bool on;

	public override void Interact()
	{
		base.Interact();
		Toggle();
	}
	
	public void Start()
	{
		SetSprite();
	}

	//	toggle on/off
	public void Toggle()
	{	
		on = !on;
		SetSprite();
	}

	//	set sprite
	private void SetSprite()
	{
		if (on)
		{
			sprite.sprite = onSprite;
		}
		else
		{
			sprite.sprite = offSprite;
		}
	}
}
