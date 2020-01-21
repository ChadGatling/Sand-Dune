using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
[System.Serializable]
public class WheelComponent {
    public WheelCollider wheel;
	/// <summary>
	/// Is this wheel attached to motor?
	/// </summary>
    public bool isPowered;
	/// <summary>
	/// Does this wheel apply steer angle?
	/// </summary>
    public bool isSteerable;
	/// <summary>
	/// Is this a front wheel?
	/// </summary>
	public bool isFore;
	/// <summary>
	/// Is this a starboard wheel?
	/// </summary>
	public bool isStarboard;
}

public class MoveShip : DriveComponent {

	Rigidbody shipPhysics;
	[SerializeField] private float Knots;
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
    [SerializeField] List<WheelComponent> wheels; // the information about each individual wheel
    // [SerializeField] float maxMotorTorque; // maximum torque the motor can apply to wheel
    [SerializeField] float maxSteeringAngle; // maximum steer angle the wheel can have
    // [SerializeField] float maxBraking; // maximum steer angle the wheel can have
	[SerializeField] DriveComponent fuelSide;
	[SerializeField] AnimationCurve frictionCurve;

	void Start () {
		shipPhysics = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
		// input = fuelSide.downStream;
		turn = Mathf.Clamp(turn, turnRange[0], turnRange[1]);
		// throttle = Mathf.Clamp(throttle, throttleRange[0], throttleRange[1]);
		// clutch = Mathf.Clamp(clutch, clutchRange[0], clutchRange[1]);
		braking = Mathf.Clamp(braking, brakingRange[0], brakingRange[1]);

        torqueCurrent = upStream.torqueCurrent;
        float steering = maxSteeringAngle * turn;
        // float brake = 100f;
		int reversing = reverse ? -1 : 1;

		wheelRPM = 0;

		float sprungMass = 0;

        foreach (WheelComponent wheelUnit in wheels) {
			float frictionTorque = frictionCurve.Evaluate(wheelUnit.wheel.rpm);

			wheelRPM += wheelUnit.wheel.rpm;
			sprungMass = wheelUnit.wheel.sprungMass;

            if (wheelUnit.isSteerable) {
				if (wheelUnit.isFore) {
					wheelUnit.wheel.steerAngle = steering;
				} else {
					wheelUnit.wheel.steerAngle = -steering;
				}
            }

			if (wheelUnit.isPowered) {
				if (upStream.isRunning) {
					wheelUnit.wheel.motorTorque = torqueCurrent - frictionTorque;
					wheelUnit.wheel.brakeTorque = frictionTorque;
				} else {
					wheelUnit.wheel.motorTorque = 0;
					wheelUnit.wheel.brakeTorque = torqueCurrent + frictionTorque;
				}
            }			
            ApplyLocalPositionToVisuals(wheelUnit.wheel);
        }

		rpmCurrent = wheelRPM / wheels.Count;
		sprungMass = sprungMass / wheels.Count;
		// fuelSide.currentRpm = ;
		Knots = Mathf.Round(shipPhysics.velocity.magnitude * 1.943844f * 100) / 100;
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
