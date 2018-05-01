using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EnemyEntity
{
    public List<PatrolPoint> patrolPoints;

	private StateMachine state;

    public float movement;

    protected override void Awake()
    {
        base.Awake();

        state = new StateMachine(this);
        //state.ChangeBehaviour(new EnemyAI.Patrol(state, physics, patrolPoints));
    }

    private void Update()
    {
        state._Update();
        spriteRenderer.sprite = GetSprite();
    }

    private void FixedUpdate()
    {
        state._FixedUpdate();
    }
}
