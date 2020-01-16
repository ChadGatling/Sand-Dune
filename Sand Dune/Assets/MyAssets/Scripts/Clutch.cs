using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clutch : DriveComponent {

	LeverAction clutch;

	void Start () {
		clutch = transform.Find("Clutch/LeverArm").GetComponent<LeverAction>();
	}
	
	void Update () {
		torqueCurrent = (upStream.isRunning ? upStream.torqueCurrent : 1000f) * clutch.howActive;
		isRunning = upStream.isRunning;
		rpmCurrent = Mathf.Lerp(upStream.rpmCurrent, downStream.rpmCurrent, clutch.howActive);

		if (downStream) {
			torqueToTurnMax = torqueToTurnCurve.Evaluate(clutch.howActive) - downStream.torqueToTurnMax;
		} else {
			torqueToTurnMax = torqueToTurnCurve.Evaluate(clutch.howActive);
		}
	}
}
