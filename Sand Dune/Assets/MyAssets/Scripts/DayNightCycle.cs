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
	[SerializeField] Material skyboxNight;
	[SerializeField] AnimationCurve intensity;
	[Range(0,1)] [SerializeField] float skyboxRatio = 0;

	private float sunSeasonAngle;
	
	void Update () {
		float dayNightCurveValue = Utility.EvaluateCurve(intensity, dayPercent);
		if (currentSecondsOfTheDay >= dayTotalSeconds)
			currentSecondsOfTheDay = currentSecondsOfTheDay - dayTotalSeconds;			
		currentSecondsOfTheDay += Time.deltaTime * dayTotalSeconds / cycleLength;
		dayPercent = currentSecondsOfTheDay/dayTotalSeconds;
		transform.eulerAngles = new Vector3((dayPercent * 360) - 90f, 0f, 0f);

		sun.intensity = dayNightCurveValue;

		// DynamicGI.UpdateEnvironment();

		// RenderSettings.skybox = dayNightCurveValue == 0 ? skyboxNight : skyboxDay; // need to smoothly blend
		// RenderSettings.skybox.Lerp(skyboxNight, skyboxDay, skyboxRatio);
		// RenderSettings.skybox.SetFloat("_Rotation", );

		timeOfDay = Mathf.Floor(currentSecondsOfTheDay / 3600).ToString("00") + ":" + (Mathf.Floor(currentSecondsOfTheDay / 60) - Mathf.Floor(currentSecondsOfTheDay / 3600) * 60).ToString("00");
	}

	void SunSet() {

	}

	void SunRise() {
		
	}
}
