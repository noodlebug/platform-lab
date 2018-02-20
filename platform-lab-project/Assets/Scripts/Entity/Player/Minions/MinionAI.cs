using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinionAI
{
	//	base behaviour class
	public class Behaviour
	{
		//	state machine running the behavious
		protected StateMachine machine;
		//	physics script used by the entity
		protected ClassicPhysics physics;
		//	entity transform	
		protected Transform mover;
		public Behaviour(StateMachine _machine, ClassicPhysics _physics)
		{
			machine = _machine;
			physics = _physics;
			mover = _physics.mover.transform;
		}

		//	Enter() and Exit() are always run by the state machine
		public void Enter()
		{
			//	do stuff
			EnterDirived();
		}

		public void Exit()
		{
			ExitDirived();
			//	do stuff
		}

		//	used in dirived class if needed
		protected virtual void EnterDirived() { }
		protected virtual void ExitDirived(){ }

		public virtual void _Update() { }
		public virtual void _FixedUpdate() { }
		
	}

	//	behaviour classes
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
			if (point.x > mover.position.x)
			{
				leftRightModifier = 1;
			}
			else
			{
				leftRightModifier = -1;
			}

			//	reached destination
			if ((point.x - mover.position.x) * leftRightModifier < randomFinish)
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
