using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clutch : DriveComponent {

	LeverAction clutch;

	void Start () {
		clutch = transform.Find("Clutch/LeverArm").GetComponent<LeverAction>();
	}
	
	void Update () {
		torque = (upStream.isRunning ? upStream.torque : 1000f) * clutch.howActive;
		isRunning = upStream.isRunning;
		rpm = Mathf.Lerp(upStream.rpm, downStream.rpm, clutch.howActive);

		if (downStream) {
			torqueToTurn = torqueToTurnCurve.Evaluate(clutch.howActive) - downStream.torqueToTurn;
		} else {
			torqueToTurn = torqueToTurnCurve.Evaluate(clutch.howActive);
		}
	}
}
