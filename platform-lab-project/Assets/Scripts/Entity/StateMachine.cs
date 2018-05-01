using System.Collections;
using System.Collections.Generic;

public class StateMachine
{
	public Entity entity;
	public Behaviour defaultBehaviour;
	
	public Behaviour currentBehaviour;	

	public StateMachine(Entity _entity)
	{
		entity = _entity;
	}
	
	//	change current behaviour
	public void ChangeBehaviour(Behaviour newBehaviour)
	{	
		if (currentBehaviour != null)
		{
			currentBehaviour.Exit();			
		}

		currentBehaviour = newBehaviour;
		currentBehaviour.Enter();
	}

	//	clear current behaviour
	public void EndBehaviour()
	{
		currentBehaviour.Exit();
		currentBehaviour = defaultBehaviour;
	}

	//	push update and fixedupdate to current behaviour
	public void _Update()
	{
		if (currentBehaviour != null)
		{
			currentBehaviour._Update();
		}
	}

	public void _FixedUpdate()
	{
		if (currentBehaviour != null)
		{
			currentBehaviour._FixedUpdate();
		}
		
	}

}
