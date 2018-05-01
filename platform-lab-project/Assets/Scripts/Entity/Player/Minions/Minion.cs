using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : PlayerEntity
{
	public PlayerController player;

    //  statemachine used to manage ai behaviours
    public StateMachine ai;

	protected override void Awake()
    {
        base.Awake();
        //physics = new ClassicPhysics(this.gameObject, physicsModifiers);
    }

	protected override void Update()
    {
        //  push update to ai
        ai._Update();

        base.Update();
    }

    private void FixedUpdate()
    {
        //  push fixed update to ai
        ai._FixedUpdate();

        //physics.Physics();
    }
    
    //  enter move to ai behaviour
    public void MoveToPoint(Vector2 point)
    {
        //ai.ChangeBehaviour(new MinionAI.MoveTo(ai, physics, point));
    }
}
