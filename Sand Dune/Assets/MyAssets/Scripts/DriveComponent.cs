using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveComponent : MonoBehaviour {

	public DriveData output = new DriveData();
	public DriveData input = new DriveData();

}

public class DriveData {
	public float rpm;
	public float torque;
}