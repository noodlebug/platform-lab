using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
	public Color color;
	
	void OnDrawGizmos()
	{
		Gizmos.color = color;
		Gizmos.DrawWireSphere(transform.position, 0.2f);
	}
}
