using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveComponent : MonoBehaviour {
	public DriveComponent downStream;
	public DriveComponent upStream;
	public AnimationCurve torqueToTurnCurve;
	public float rpm;
	public float torque;
	public float torqueToTurn;
	[HideInInspector] public bool isRunning;
}

// [System.Serializable]
// public class DriveData {
// 	public float rpm;
// 	public float torque;
// }