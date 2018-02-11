using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public GameManager game;
	public Camera cam;
	public Canvas canvas;

	[Header("Interactables")]
	[HideInInspector]public InteractableUI interactable;

	[Header("Highlights")]
	[HideInInspector]public HighlightManager highlight;
	public GameObject highlightPrefab;

	private void Start()
	{
		interactable = new InteractableUI(this);
		highlight = new HighlightManager(this, highlightPrefab);
	}

	private void LateUpdate()
	{
		//	push Update() to highlight manager
		highlight.UpdateHighlights();
	}
}
