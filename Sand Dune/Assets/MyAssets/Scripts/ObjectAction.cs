using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAction : MonoBehaviour {

	public bool isActive = false;
	public float howActive = 0;
	[HideInInspector] public static float rangeMax;
	[HideInInspector] public float rangeMin;
	public float angleMax;

	public virtual void Action (RaycastHit hit) {
	}
}
