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
		float unclampedTorque;
		var rpmRatio = rpmCurrent/rpmMax;

		if (downStream != null) unclampedTorque = Utility.EvaluateCurve(torqueCurve, throttle.howActive, torqueMax) - torqueToTurnMax - downStream.torqueToTurnMax;
		else unclampedTorque = Utility.EvaluateCurve(torqueCurve, rpmRatio, torqueMax) * throttle.howActive - Utility.EvaluateCurve(torqueToTurnCurve, rpmRatio, torqueToTurnMax);
		
		torqueCurrent = Mathf.Clamp(unclampedTorque, 0, torqueMax); // Need to add negative torque to the engine based on the ratio of the throttle (ratio) to the rpm (ratio).
	}

	void SetRpm() {
 		if (downStream != null) {
			float clutchHowActive = downStream.transform.Find("Clutch/LeverArm").GetComponent<LeverAction>().howActive;			 
			rpmCurrent = Mathf.Lerp(Utility.EvaluateCurve(rpmCurve, torqueCurrent), downStream.rpmCurrent, clutchHowActive);
		} else rpmCurrent += (torqueCurrent - Utility.EvaluateCurve(torqueToTurnCurve, rpmCurrent/rpmMax, torqueToTurnMax)) * Time.deltaTime;

		rpmCurrent = Mathf.Clamp(rpmCurrent, 0, rpmMax);
		
        ParticleSystem.EmissionModule emission = exhaust.emission;
		emission.rateOverTime = rpmCurrent;
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