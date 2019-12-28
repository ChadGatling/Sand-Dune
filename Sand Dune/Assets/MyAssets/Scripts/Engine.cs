using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : DriveComponent {

	public float maxTorque;
	public float startTorque;
	public AnimationCurve rpmCurve;
	public AnimationCurve torqueCurve;

	float minRpm;
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
			SetTorque();
			SetRpm();
			IncreaseTemperature();
			Stall();
		} else {
			rpm = 0;
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
		torqueToTurn = torqueToTurnCurve.Evaluate(throttle.howActive);

		if (downStream)
			unclampedTorque = torqueCurve.Evaluate(throttle.howActive) - torqueToTurn - downStream.torqueToTurn;
		else
			unclampedTorque = torqueCurve.Evaluate(throttle.howActive) - torqueToTurn;

		torque = Mathf.Clamp(unclampedTorque, 0, maxTorque);
	}

	void SetRpm() {
		float clutchHowActive = downStream.transform.Find("Clutch/LeverArm").GetComponent<LeverAction>().howActive;

		if (downStream) {
			rpm = Mathf.Lerp(rpmCurve.Evaluate(torque), downStream.rpm, clutchHowActive);
		} else {			
			rpm = rpmCurve.Evaluate(torque);
		}
		
        ParticleSystem.EmissionModule emission = exhaust.emission;
		emission.rateOverTime = rpm;
	}

	void Ignition() {
		if (!ignition.isActive) {
			torque = 0;
			return;
		}
		
		torque += Time.deltaTime * startTorque;
		torque = Mathf.Clamp(torque, 0, startTorque);
		
		if (choke.isActive && torque > torqueToTurn) {
			isRunning = true;
			exhaust.Play();
		}
	}

	void Stall() {
		if (torque < torqueToTurn) {
			isRunning = false;
			exhaust.Stop();
		}
	}

	void SetOutput() {
	}
}