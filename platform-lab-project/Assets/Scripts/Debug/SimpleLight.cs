using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//	color changing sprite
public class SimpleLight : MonoBehaviour
{
	public SpriteRenderer sprite;
	public Color onColor;
	public Color offColor;
	public bool on;

	private void Awake()
	{
		SetColor();
	}
	
	//	toggle on/off
	public void Toggle()
	{	
		on = !on;
		SetColor();
	}

	//	set sprite color
	private void SetColor()
	{
		if (on)
		{
			sprite.color = onColor;
		}
		else
		{
			sprite.color = offColor;
		}
	}
}
