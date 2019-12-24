using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : DriveComponent {

	public float currentRpm;
	public float maxRpm;
	public float minRpm;
	public float currentTorque;
	public float maxTorque;
	public float minTorque;
	public bool isRunning;
	public float startTorque;

	LeverAction throttle;
	ButtonAction ignition;
	PushPullAction choke;
	ParticleSystem exhaust;
	int temperature = 70;

	void Start () {
		throttle = transform.Find("Throttle/LeverArm").GetComponent<LeverAction>();
		ignition = transform.Find("Ignition").GetComponent<ButtonAction>();
		choke = transform.Find("Choke").GetComponent<PushPullAction>();
		exhaust = GetComponent<ParticleSystem>();
		SetOutput();
	}
	
	void Update () {
		Ignition();
		setTemperature();
		setRpm();
		Stall();
		SetOutput();
	}

	void setTemperature() {
		
	}

	void setRpm() {		
		if (!isRunning)
		return;

		currentRpm = Mathf.Lerp(0, maxRpm, throttle.howActive);
        ParticleSystem.EmissionModule emission = exhaust.emission;
		emission.rateOverTime = currentRpm;
	}

	void Ignition() {
		if (isRunning)
			return;

		if (!ignition.isActive) {
			currentRpm = 0;
			return;
		}

		currentRpm += Time.deltaTime * startTorque;
		currentRpm = Mathf.Clamp(currentRpm, 0, minRpm * 1.05f);
		
		if (choke.isActive && currentRpm >= minRpm) {
			isRunning = true;
			exhaust.Play();
		}
	}

	void Stall() {
		if (currentRpm < minRpm) {
			isRunning = false;			
			exhaust.Stop();
		}
	}

	void SetOutput() {
		output.rpm = currentRpm;
		output.torque = currentTorque;
	}
}