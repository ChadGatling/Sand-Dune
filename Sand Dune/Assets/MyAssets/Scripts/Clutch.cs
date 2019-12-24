using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clutch : DriveComponent {

	public DriveComponent inputObject;

	LeverAction clutch;

	void Start () {
		clutch = transform.Find("Clutch/LeverArm").GetComponent<LeverAction>();
	}
	
	void Update () {
		input = inputObject.output;
		output.rpm = input.rpm * clutch.howActive;
	}
}
