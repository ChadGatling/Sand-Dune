using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
[System.Serializable]
public class AxleInfo {
    public WheelCollider portWheel;
    public WheelCollider starboardWheel;
    public bool isPowered; // is this wheel attached to motor?
    public bool isSteerable; // does this wheel apply steer angle?
	public bool isFore; // is the wheel in thr front?
}

public class MoveShip : DriveComponent {

	Rigidbody shipPhysics;
	// [Range(0f, 30f)] // make the high end the same as maxRPG
	float wheelRPM;
	// [SerializeField] float maxRPM;
	[HideInInspector] public float[] turnRange = {-1f, 1f};
	// [HideInInspector] public float[] throttleRange = {0f, 1f};
	// [HideInInspector] public float[] clutchRange = {0f, 1f};
	[HideInInspector] public float[] brakingRange = {0f, 1f};
	[Range(-1f, 1f)] public float turn;
	// [Range(0f, 1f)]	public float throttle;
	// [Range(0f, 1f)]	public float clutch;
	[Range(0, 1f)] public float braking;
	[SerializeField] bool reverse;
    [SerializeField] List<AxleInfo> axleInfos; // the information about each individual axle
    // [SerializeField] float maxMotorTorque; // maximum torque the motor can apply to wheel
    [SerializeField] float maxSteeringAngle; // maximum steer angle the wheel can have
    // [SerializeField] float maxBraking; // maximum steer angle the wheel can have
	[SerializeField] DriveComponent fuelSide;
	[SerializeField] AnimationCurve frictionCurve;

	void Start () {
		shipPhysics = GetComponent<Rigidbody>();
	}
	
	void Update () {
		// input = fuelSide.downStream;
		turn = Mathf.Clamp(turn, turnRange[0], turnRange[1]);
		// throttle = Mathf.Clamp(throttle, throttleRange[0], throttleRange[1]);
		// clutch = Mathf.Clamp(clutch, clutchRange[0], clutchRange[1]);
		braking = Mathf.Clamp(braking, brakingRange[0], brakingRange[1]);

        torque = upStream.torque;
        float steering = maxSteeringAngle * turn;
        // float brake = 100f;
		int reversing = reverse ? -1 : 1;

		wheelRPM = 0;

		float sprungMass = 0;

        foreach (AxleInfo axleInfo in axleInfos) {

			wheelRPM = axleInfo.portWheel.rpm + axleInfo.starboardWheel.rpm;
			sprungMass = axleInfo.portWheel.sprungMass + axleInfo.starboardWheel.sprungMass;

			axleInfo.portWheel.brakeTorque = frictionCurve.Evaluate(axleInfo.portWheel.rpm);
			axleInfo.starboardWheel.brakeTorque = frictionCurve.Evaluate(axleInfo.starboardWheel.rpm);

            if (axleInfo.isSteerable) {
				if (axleInfo.isFore) {
					axleInfo.portWheel.steerAngle = steering;
					axleInfo.starboardWheel.steerAngle = steering;
				} else {
					axleInfo.portWheel.steerAngle = -steering;
					axleInfo.starboardWheel.steerAngle = -steering;
				}
            }

			// if (braking > 0) {
			// 	axleInfo.portWheel.brakeTorque = brake;
			// 	axleInfo.starboardWheel.brakeTorque = brake;
			// } else 
			if (axleInfo.isPowered) {
				if (upStream.isRunning) {
					axleInfo.portWheel.motorTorque = torque;
					axleInfo.starboardWheel.motorTorque = torque;
				} else {
					axleInfo.portWheel.brakeTorque = torque + frictionCurve.Evaluate(axleInfo.portWheel.rpm); // Being weird. if engine is killed when running and then the clutch is fully disengaged the wheels want to power forward. IDK!
					axleInfo.starboardWheel.brakeTorque = torque + frictionCurve.Evaluate(axleInfo.portWheel.rpm);
				}

				// if (axleInfo.portWheel.rpm < upStream.rpm) {
				// 	axleInfo.portWheel.brakeTorque = 0f;
				// 	axleInfo.portWheel.motorTorque = motor;
				// } else if (axleInfo.portWheel.rpm > upStream.rpm) {
				// 	axleInfo.portWheel.motorTorque = 0f;
				// 	axleInfo.portWheel.brakeTorque = brake;
				// } else {
				// 	axleInfo.portWheel.brakeTorque = 0f;
				// 	axleInfo.portWheel.motorTorque = 0f;
				// }

				// if (axleInfo.starboardWheel.rpm < upStream.rpm) {
				// 	axleInfo.starboardWheel.brakeTorque = 0f;
				// 	axleInfo.starboardWheel.motorTorque = motor;
				// } else if (axleInfo.starboardWheel.rpm > upStream.rpm) {
				// 	axleInfo.starboardWheel.motorTorque = 0f;
				// 	axleInfo.starboardWheel.brakeTorque = brake;
				// } else {
				// 	axleInfo.starboardWheel.brakeTorque = 0f;
				// 	axleInfo.starboardWheel.motorTorque = 0f;
				// }
            }			
            ApplyLocalPositionToVisuals(axleInfo.portWheel);
            ApplyLocalPositionToVisuals(axleInfo.starboardWheel);
        }

		rpm = wheelRPM / (axleInfos.Count * 2);
		sprungMass = sprungMass / (axleInfos.Count * 2);
		// fuelSide.currentRpm = ;
		Debug.Log((Mathf.Round(shipPhysics.velocity.magnitude * 1.943844f * 100) / 100) + " Knots");
	}
	
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
     
        Transform visualWheel = collider.transform.GetChild(0);
     
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
     
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}
