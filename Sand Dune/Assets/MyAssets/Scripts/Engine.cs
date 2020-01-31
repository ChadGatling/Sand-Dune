using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : DriveComponent {

	[SerializeField] float torqueMax;
	[SerializeField] float torqueStart;
	[SerializeField] AnimationCurve torqueCurve;

	[SerializeField] float rpmMax;
	[SerializeField] float rpmMin;
	// [SerializeField] AnimationCurve rpmCurve;

	LeverAction throttle;
	ButtonAction ignition;
	PushPullAction choke;
	ParticleSystem exhaust;
	[SerializeField] float temperature = 70;

	float startTime;

	void Start () {
		throttle = transform.Find("Throttle/LeverArm").GetComponent<LeverAction>();
		ignition = transform.Find("Ignition").GetComponent<ButtonAction>();
		choke = transform.Find("Choke").GetComponent<PushPullAction>();
		exhaust = GetComponent<ParticleSystem>();
		
		SetOutput();
	}
	
	void FixedUpdate () {
		if (isRunning) {
			IncreaseTemperature();
			SetRpm();
			Stall();
			SetTorque();
		} else {
			rpmCurrent = 0;
			Ignition();
			DecreaseTemperature();
		}

		SetOutput();
	}

	void IncreaseTemperature() {
		if (temperature < 250) {
			temperature += Time.fixedDeltaTime;
			temperature = Mathf.Clamp(temperature, 70, 250);
		}
	}

	void DecreaseTemperature() {
		if (temperature > 70) {
			temperature -= Time.fixedDeltaTime;
			temperature = Mathf.Clamp(temperature, 70, 250);
		}
	}

	void SetTorque() {
		var rpmRatio = rpmCurrent/rpmMax;
		// var torqueMaxThisRpm = Utility.EvaluateCurve(torqueCurve, rpmRatio, torqueMax);
		// float engineBrakeTorque = torqueMaxThisRpm - torqueMaxThisRpm * throttle.howActive;
		float torqueGross = Utility.EvaluateCurve(torqueCurve, rpmRatio, torqueMax) * throttle.howActive;		
		torqueCurrent = (torqueGross) - Utility.EvaluateCurve(torqueToTurnCurve, rpmRatio, torqueToTurnMax) - 1; // add torque to turn friction as the unit wears

		if (downStream != null) {
			float clutchHowActive = downStream.transform.Find("Clutch/LeverArm").GetComponent<LeverAction>().howActive;
			torqueCurrent -= downStream.torqueToTurnMax * clutchHowActive;  
		}

		if (torqueCurrent < 0)
			torqueCurrent *= 3; // might be as good as I can get for now. Meant to simulate engine brake.
		
		if (!isRunning)
			torqueCurrent *= 5;

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
			rpmCurrent = Mathf.Lerp(rpmCurrent + torqueCurrent * Time.fixedDeltaTime * 20, downStream.rpmCurrent, clutchHowActive);
		} else
			rpmCurrent += torqueCurrent * Time.fixedDeltaTime * 20;

		rpmCurrent = Mathf.Clamp(rpmCurrent, 0, rpmMax);
		
        ParticleSystem.EmissionModule emission = exhaust.emission;
		emission.rateOverTime = rpmCurrent / 60;
	}

	void Ignition() {
		if (!ignition.isActive) {
			// torqueCurrent = 0;
			startTime = 0;
			return;
		}
		
		if (startTime <= 0.5) {
			startTime += Time.fixedDeltaTime;
			torqueCurrent += torqueStart * Time.fixedDeltaTime * 2;
			torqueCurrent = Mathf.Clamp(torqueCurrent, 0, torqueStart);
		}

		SetRpm();
		
		if (choke.isActive && rpmCurrent >= rpmMin) {
			isRunning = true;
			exhaust.Play();
		}
	}

	void Stall() {
		if (rpmCurrent <= 0 || throttle.howActive <= 0) {
			isRunning = false;
			exhaust.Stop();
		}
	}

	void SetOutput() {
	}
}