using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinionAI
{
	//	move to point
	public class MoveTo : Behaviour
	{
		//	target destination (direction)
		private Vector2 point;
		private float axis;

		private float randomFinish;
		private float accelTime = 0.5f;
		private float startTime;
		public MoveTo(StateMachine _machine, ClassicPhysics _physics, Vector2 _point) : base(_machine, _physics)
		{
			point = _point;
		}

		protected override void EnterDirived()
		{
			randomFinish = Random.Range(0.1f, 1f);
			startTime = Time.time;
		}

		public override void _Update()
		{
			float accel = Mathf.SmoothStep(0, 1, Time.time - startTime / accelTime);
			int leftRightModifier;
			
			if (point.x > transform.position.x)
			{
				leftRightModifier = 1;
			}
			else
			{
				leftRightModifier = -1;
			}

			//	reached destination
			if ((point.x - transform.position.x) * leftRightModifier < randomFinish)
			{
				machine.EndBehaviour();
				return;
			}

			//	move towards destination
			physics.Input(leftRightModifier);
		}

		protected override void ExitDirived()
		{
			physics.Input(0);			
		}
	}
}
