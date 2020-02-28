using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {

	[SerializeField] string timeOfDay;
	[SerializeField] float cycleLength;
	[SerializeField] float currentSecondsOfTheDay;
	[SerializeField] float dayTotalSeconds = 86400;
	[SerializeField] float dayPercent;
	[SerializeField] Light sun;
	[SerializeField] Material skyboxDay;
	[SerializeField] Material skyboNight;
	[SerializeField] AnimationCurve intensity;

	private float sunSeasonAngle;
	
	void Update () {
		float dayNightCurveValue = Utility.EvaluateCurve(intensity, dayPercent);
		if (currentSecondsOfTheDay >= dayTotalSeconds)
			currentSecondsOfTheDay = currentSecondsOfTheDay - dayTotalSeconds;			
		currentSecondsOfTheDay += Time.deltaTime * dayTotalSeconds / cycleLength;
		dayPercent = currentSecondsOfTheDay/dayTotalSeconds;
		transform.eulerAngles = new Vector3((dayPercent * 360) - 90f, 0f, 0f);

		sun.intensity = dayNightCurveValue;

		RenderSettings.skybox = dayNightCurveValue == 0 ? skyboNight : skyboxDay; // need to smoothly blend

		// This is probably overkill. maybe just make this a animationcurve
		// if (dayPercent <= .255)
		// 	sun.intensity = 100 * Mathf.Abs(dayPercent - .25f) + 100 * (dayPercent - .25f);
		// else if (dayPercent >= .745)
		// 	sun.intensity = (-100 * Mathf.Abs(dayPercent - .745f) - 100 * (dayPercent - .745f)) + 1;
		// else
		// 	sun.intensity = 1;

		timeOfDay = Mathf.Floor(currentSecondsOfTheDay / 3600).ToString("00") + ":" + (Mathf.Floor(currentSecondsOfTheDay / 60) - Mathf.Floor(currentSecondsOfTheDay / 3600) * 60).ToString("00");
	}

	void SunSet() {

	}

	void SunRise() {
		
	}
}
