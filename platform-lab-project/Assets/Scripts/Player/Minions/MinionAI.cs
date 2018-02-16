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
		public MoveTo(StateMachine _machine, ClassicPhysics _physics, Vector2 _point) : base(_machine, _physics)
		{
			point = _point;
		}

		public override void _Update()
		{
			//	reached destination
			if (Vector2.Distance(point, mover.position) > 1)
			{
				machine.Kill();
				return;
			}

			//	move towards destination
			if (point.x > mover.position.x)
			{
				machine.game.debug.Log(mover.position.x.ToString(), point.x.ToString());
				//	right
				physics.Input(4);				
			}
			else
			{
				//	left
				physics.Input(-4);								
			}

		}
	}
}
