using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  the root of the game

public class GameManager : MonoBehaviour
{
    public PlayerController player;
	public BallController ball;
	public PlayerEntity activePlayerEntity;

	public Camera cam;
	public UIManager ui;
	public GameObject dedText;
	
	public float cameraFollowRange = 2;
    
	//  debug
    public DebugThings debug;
    public Transform debugPanel;
	public bool gizmosOn = false;

    private void Awake()
    {
		ui.game = this;
        debug = new DebugThings(debugPanel, debugPanel.GetComponentInChildren<Text>());
		activePlayerEntity = player;
    }

    private void Update()
    {
        //  push Update() to non Monobehaviour class DebugThings
        debug.Refresh();
		CameraFollow(cameraFollowRange);
    }

	// draw gizmos
    private void OnDrawGizmos()
    {
		if (!gizmosOn)
		{
			return;
		}
		if (debug.lines.Count == 0)
		{
			return;
		}

        Gizmos.color = Color.yellow;

        // draw lines        
        foreach (KeyValuePair<string, Vector2[]> kvp in debug.lines)
        {
            Gizmos.DrawLine(kvp.Value[0], kvp.Value[1]);
        }
    }

	public void ChangeEntity()
	{
		if (activePlayerEntity == player)
		{
			activePlayerEntity = ball;
			ball.spriteRenderer.color = Color.green;
			player.spriteRenderer.color = Color.white;
		}
		else
		{
			activePlayerEntity = player;
			ball.spriteRenderer.color = Color.white;
			player.spriteRenderer.color = Color.green;
		}
	}

	public void KillPlayer()
	{
		//	pause game and display game over text
		dedText.SetActive(true);
		Time.timeScale = 0;
	}
	
	//  camera follow
	private void CameraFollow(float maxDistance)
	{
		//	get player distance from center
		float x = activePlayerEntity.transform.position.x - cam.transform.position.x;
		float y = activePlayerEntity.transform.position.y - cam.transform.position.y;

		Vector2 translation = new Vector2();

		//	adjust vector if player to far away from center
		if (x > maxDistance)
		{
			translation.x = x - maxDistance;
		}
		else if (x < -maxDistance)
		{
			translation.x = x + maxDistance;;
		}

		//	more sensitive on y axis
		maxDistance /= 2;

		if (y > maxDistance)
		{
			translation.y = y - maxDistance;
		}
		else if (y < -maxDistance)
		{
			translation.y = y + maxDistance;;
		}

		//return if nothing to adjust
		if (translation == Vector2.zero)
		{
			return;
		}

		
		//	adjust camera position
		cam.transform.Translate(translation);
	}
}