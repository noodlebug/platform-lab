using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//	interactable button in game world

public class GameButton : Interactable
{
	public SpriteRenderer sprite;
	public Sprite onSprite;
	public Sprite offSprite;
	public bool on;

	public override void Interact()
	{
		base.Interact();
		Toggle();
	}
	
	public void Awake()
	{
		SetColor();
	}

	//	toggle on/off
	public void Toggle()
	{	
		on = !on;
		SetColor();
	}

	//	set sprite
	private void SetColor()
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
