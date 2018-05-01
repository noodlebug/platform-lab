using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour
{
	//	state machine running the behaviours
	protected StateMachine machine;
	//	physics script used by the entity
	protected ClassicPhysics physics;
	//	entity transform	
	protected Transform transform;
	
	public Behaviour(StateMachine _machine, ClassicPhysics _physics)
	{
		machine = _machine;
		physics = _physics;
		transform = _physics.gObj.transform;
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
