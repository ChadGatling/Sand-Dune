using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {

	public float cycleLength;

	private float sunSeasonAngle;
	
	void Update () {
		transform.Rotate(Time.deltaTime * 360/cycleLength, 0f, 0f);
	}
}
