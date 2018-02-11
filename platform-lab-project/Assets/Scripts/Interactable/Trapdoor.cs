using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//	simple trapdoor

public class Trapdoor : MonoBehaviour
{
	public GameObject leftPivot;
	public GameObject rightPivot;

	public bool open = false;
	public void Toggle()
	{
		int adjust = open ? -90 : 90;
		open = !open;

		leftPivot.transform.Rotate(new Vector3(0, 0, -adjust));
		rightPivot.transform.Rotate(new Vector3(0, 0, adjust));
	}
}
