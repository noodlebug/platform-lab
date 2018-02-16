using System.Collections;
using System.Collections.Generic;

public class StateMachine
{
	public GameManager game;
	
	public StateMachine(GameManager _game)
	{
		game = _game;
	}
	
	public MinionAI.Behaviour currentBehaviour;

	//	change current behaviour
	public void ChangeBehaviour(MinionAI.Behaviour newBehaviour)
	{	
		if (currentBehaviour != null)
		{
			currentBehaviour.Exit();			
		}

		currentBehaviour = newBehaviour;
		currentBehaviour.Enter();
	}

	//	clear current behaviour
	public void Kill()
	{
		currentBehaviour.Exit();
		currentBehaviour = null;
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
