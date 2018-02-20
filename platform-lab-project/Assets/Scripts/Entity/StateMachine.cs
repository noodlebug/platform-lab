using System.Collections;
using System.Collections.Generic;

public class StateMachine
{
	public GameManager game;
	public MinionAI.Behaviour defaultBehaviour;
	
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
		game.debug.Log("AI state: ", currentBehaviour.ToString());
		UnityEngine.Debug.Log("changed state: " + currentBehaviour.ToString());
	}

	//	clear current behaviour
	public void EndBehaviour()
	{
		currentBehaviour.Exit();
		currentBehaviour = null;
		game.debug.Log("AI state: ", "");
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
