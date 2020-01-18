using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : DriveComponent {

	[SerializeField] float torqueMax;
	[SerializeField] float torqueStart;
	[SerializeField] AnimationCurve torqueCurve;

	[SerializeField] float rpmMax;
	[SerializeField] float rpmMin;
	[SerializeField] AnimationCurve rpmCurve;

	LeverAction throttle;
	ButtonAction ignition;
	PushPullAction choke;
	ParticleSystem exhaust;
	[SerializeField] float temperature = 70;

	void Start () {
		throttle = transform.Find("Throttle/LeverArm").GetComponent<LeverAction>();
		ignition = transform.Find("Ignition").GetComponent<ButtonAction>();
		choke = transform.Find("Choke").GetComponent<PushPullAction>();
		exhaust = GetComponent<ParticleSystem>();
		
		SetOutput();
	}
	
	void Update () {
		if (isRunning) {
			SetRpm();
			SetTorque();
			IncreaseTemperature();
			Stall();
		} else {
			rpmCurrent = 0;
			Ignition();
			DecreaseTemperature();
		}

		SetOutput();
	}

	void IncreaseTemperature() {
		if (temperature < 250) {
			temperature += Time.deltaTime;
			temperature = Mathf.Clamp(temperature, 70, 250);
		}
	}

	void DecreaseTemperature() {
		if (temperature > 70) {
			temperature -= Time.deltaTime;
			temperature = Mathf.Clamp(temperature, 70, 250);
		}
	}

	void SetTorque() {
		var rpmRatio = rpmCurrent/rpmMax;
		// var torqueMaxThisRpm = Utility.EvaluateCurve(torqueCurve, rpmRatio, torqueMax);
		// float engineBrakeTorque = torqueMaxThisRpm - torqueMaxThisRpm * throttle.howActive;
		float torqueGross = Utility.EvaluateCurve(torqueCurve, rpmRatio, torqueMax) * throttle.howActive;

		if (downStream != null) {
			float clutchHowActive = downStream.transform.Find("Clutch/LeverArm").GetComponent<LeverAction>().howActive;
			torqueCurrent = torqueGross - Utility.EvaluateCurve(torqueToTurnCurve, rpmRatio, torqueToTurnMax) - 1 - downStream.torqueToTurnMax * clutchHowActive;  
		} else torqueCurrent = (torqueGross) - Utility.EvaluateCurve(torqueToTurnCurve, rpmRatio, torqueToTurnMax) - 1;

		if (torqueCurrent < 0) torqueCurrent *= 3; // might be as good as I can get for now. Meant to simulate engine brake.

		#if UNITY_EDITOR
		if (torqueCurrent > torqueMax) { // Remove after some time when it is shown that it will not happen.
			Debug.Break();
			Debug.Log("torqueCurrent has exceded torqueMax");
			torqueCurrent = torqueMax;
		};
		#endif
	}

	void SetRpm() {
 		if (downStream != null) {
			float clutchHowActive = downStream.transform.Find("Clutch/LeverArm").GetComponent<LeverAction>().howActive;
			rpmCurrent = Mathf.Lerp( rpmCurrent + torqueCurrent * Time.deltaTime * 50, downStream.rpmCurrent, clutchHowActive);
		} else rpmCurrent += torqueCurrent * Time.deltaTime * 50;

		rpmCurrent = Mathf.Clamp(rpmCurrent, 0, rpmMax);
		
        ParticleSystem.EmissionModule emission = exhaust.emission;
		emission.rateOverTime = rpmCurrent / 100;
	}

	void Ignition() {
		if (!ignition.isActive) {
			torqueCurrent = 0;
			return;
		}
		
		torqueCurrent += Time.deltaTime * torqueStart;
		torqueCurrent = Mathf.Clamp(torqueCurrent, 0, torqueStart);
  		SetRpm();
		
		if (choke.isActive && torqueCurrent >= torqueStart) {
			isRunning = true;
			exhaust.Play();
		}
	}

	void Stall() {
		if (rpmCurrent <= 0) {
			isRunning = false;
			exhaust.Stop();
		}
	}

	void SetOutput() {
	}
}