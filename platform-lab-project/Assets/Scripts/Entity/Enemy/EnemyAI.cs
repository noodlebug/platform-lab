using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyAI
{
	public class Template : Behaviour
	{
		public Template(StateMachine _machine, ClassicPhysics _physics) : base(_machine, _physics)
		{
		}
	}

	public class Patrol : Behaviour
	{
		public float acceleration;

		StateMachine move;
		List<PatrolPoint> points;

		int index = -1;
		int leftRightModifier;

		private float accelerateTime = 0.5f;
		private float startTime;

		public Patrol(StateMachine _machine, ClassicPhysics _physics, List<PatrolPoint> _points) : base(_machine, _physics)
		{
			points = _points;
			move = new StateMachine(_machine.entity);
		}

		protected override void EnterDirived()
		{
			NextPoint();			
		} 

		public override void _Update()
		{
			//	convert bool to sign
			leftRightModifier = machine.entity.facingRight ? -1 : 1;

			machine.entity.game.debug.Log(
				"distance: ",
				Mathf.Abs(points[index].transform.position.x - transform.position.x).ToString() + " < " +  "0.02f");

			machine.entity.game.debug.Log("index: ", index.ToString());

			//	if close to current point
			if (Mathf.Abs(points[index].transform.position.x - transform.position.x) < 0.02f)
			{
				//	next point
				NextPoint();
				return;
			}

			//	accelerate gradually
			acceleration = Mathf.SmoothStep(0, 1, Time.time - startTime / accelerateTime) * leftRightModifier;

			//	separate movement from gravity
			physics.Input(acceleration);
		}

		public override void _FixedUpdate()
		{
			physics.Physics();
		}

		private void NextPoint()
		{
			index ++;			
			startTime = Time.time;
			Debug.Log("patrol: " + points[index].name);			
		}
	}
}
