using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {

	[SerializeField] string timeOfDay;
	[SerializeField] float cycleLength;
	[SerializeField] float currentSecondsOfTheDay;
	[SerializeField] float dayTotalSeconds = 86400;

	private float sunSeasonAngle;
	
	void Update () {
		if (currentSecondsOfTheDay >= dayTotalSeconds)
			currentSecondsOfTheDay = currentSecondsOfTheDay - dayTotalSeconds;			
		currentSecondsOfTheDay += Time.deltaTime * dayTotalSeconds / cycleLength;
		transform.eulerAngles = new Vector3((currentSecondsOfTheDay/dayTotalSeconds * 360) - 90f, 0f, 0f);

		timeOfDay = Mathf.Floor(currentSecondsOfTheDay / 3600).ToString("00") + ":" + (Mathf.Floor(currentSecondsOfTheDay / 60) - Mathf.Floor(currentSecondsOfTheDay / 3600) * 60).ToString("00");
	}
}
