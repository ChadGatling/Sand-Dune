using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveComponent : MonoBehaviour {
	[HideInInspector] public bool isRunning;
	public DriveComponent downStream;
	public DriveComponent upStream;
	public float rpmCurrent;
	public float torqueCurrent;
	public float torqueToTurnMax;
	public AnimationCurve torqueToTurnCurve;
}

// [System.Serializable]
// public class DriveData {
// 	public float rpm;
// 	public float torque;
// }