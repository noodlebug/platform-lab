using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  the root of the game

public class GameManager : MonoBehaviour
{
    public PlayerController player;
	public Camera cam;
	public UIManager ui;
	
	public float cameraFollowRange = 2;
    
	//  debug
    public DebugThings debug;
    public Transform debugPanel;

    private void Awake()
    {
        player.game = this;
		ui.game = this;
        debug = new DebugThings(debugPanel, debugPanel.GetComponentInChildren<Text>());
    }

    private void Update()
    {
        //  push Update() to non Monobehaviour class DebugThings
        debug.RefreshStrings();
    }

	private void FixedUpdate()
	{
		CameraFollow(cameraFollowRange);
	}

	//  camera follow
	private void CameraFollow(float maxDistance)
	{
		//	get player distance from center
		float x = player.transform.position.x - cam.transform.position.x;
		float y = player.transform.position.y - cam.transform.position.y;

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