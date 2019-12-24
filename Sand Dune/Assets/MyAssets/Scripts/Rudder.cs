using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rudder : DriveComponent {

	public MoveShip ship;
	LeverAction throttle;

	void Start () {
		throttle = transform.Find("Rudder/LeverArm").GetComponent<LeverAction>();
	}
	
	void Update () {
		ship.turn = throttle.howActive;
	}
}
