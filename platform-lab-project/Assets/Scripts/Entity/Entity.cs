using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Sprite rightSprite;
    public Sprite leftSprite;

    [Header("Physics")]  
    //  for normal movement, set all to 1  
    //public ClassicPhysics.Modifiers physicsModifiers;

    //  physics script
    //protected ClassicPhysics physics;

	[HideInInspector]public GameManager game;    
    [HideInInspector]public Rigidbody2D rigidBody;
    [HideInInspector]public SpriteRenderer spriteRenderer;
    [HideInInspector]public Collider2D collider_;          
    [HideInInspector]public bool facingRight;  

    //  grounded check
    private Vector2 groundNormal;
    private ContactFilter2D contactFilter;
    //	buffer radius to prevent overlapping
	const float shell = 0.02f;
    //  minimum angle to constitute ground
    private float minGroundNormalY = 0.65f;
    //  entity is grounded
    public bool grounded = false;

	protected virtual void Awake()
    {
		game = GameObject.FindObjectOfType<GameManager>();
        rigidBody= GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider_ = GetComponent<Collider2D>();
        
        //	do not detect trigger colliders, use player object collision settings
	    contactFilter.useTriggers = false;
	    contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
	}

    public Sprite SetSprite()
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

    protected bool Grounded()
	{
        //	cast player collider in direction of movement - get next frame's collisions
        RaycastHit2D[] hits = new RaycastHit2D[16];
        int count = rigidBody.Cast(new Vector2(0, -1), contactFilter, hits, rigidBody.velocity.magnitude * Time.fixedDeltaTime + shell);

        //	trim hits array to list
        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        for (int i = 0; i < count; i++)
        {
            hitList.Add(hits[i]);
        }

        //	iterate over hits (collisions next frame)
        foreach (RaycastHit2D hit in hitList)
        {
            //	copy hit normal (angle of face)
            Vector2 currentNormal = hit.normal;

            //	check how steep slope is
            if (currentNormal.y > 0.64f * minGroundNormalY)
            {
                //	player is on ground
                return true;
            }
        }
		return false;

	}
}
