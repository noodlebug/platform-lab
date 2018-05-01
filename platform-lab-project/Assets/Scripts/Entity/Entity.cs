using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Sprite rightSprite;
    public Sprite leftSprite;

    [Header("Physics")]  
    //  for normal movement, set all to 1  
    public ClassicPhysics.Modifiers physicsModifiers;

    //  physics script
    protected ClassicPhysics physics;

	[HideInInspector]public GameManager game;    
    [HideInInspector]public Rigidbody2D rigidBody;
    [HideInInspector]public SpriteRenderer spriteRenderer;
    [HideInInspector]public bool facingRight;    

	protected virtual void Awake()
    {
		game = GameObject.FindObjectOfType<GameManager>();
        rigidBody= GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        physics = new ClassicPhysics(this.gameObject, physicsModifiers);
        
	}

    public Sprite GetSprite()
    {
        if (facingRight)
        {
            return rightSprite;
        }
        else
        {
            return leftSprite;
        }
    }
}
