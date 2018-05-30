using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//	generic functions
public static class MyAPI
{
	public static Quaternion FromToRotation(Vector3 target, Vector3 origin)
	{
		Vector3 diff = target - origin;
		diff.Normalize();

		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		return Quaternion.Euler(0f, 0f, rot_z - 90);
	}
}
