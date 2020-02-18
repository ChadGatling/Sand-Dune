using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverAction : ObjectAction {

	// [SerializeField] string control;

	// System.Reflection.FieldInfo controlField;
	// MoveShip moveShip;

	[SerializeField] bool allowNegative;

	public void Start() {
		// moveShip = transform.root.GetComponent<MoveShip>();
		// System.Reflection.FieldInfo rangeField = moveShip.GetType().GetField(control + "Range");
		// float[] range = (float[])rangeField.GetValue(moveShip);
		rangeMin = allowNegative ? -1 : 0;
		rangeMax = 1;
		// controlField = moveShip.GetType().GetField(control);
	}

	void Update() {
		transform.parent.localEulerAngles = new Vector3(howActive * 60, 0, 90);
	}

	public override void Action(RaycastHit hit) {
		float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
		bool onClick = Input.GetButtonDown("Action");
		bool onHold = Input.GetButton("Action");

		// if (mouseScroll != 0) {			
		// 	// controlField = moveShip.GetType().GetField(control);
		// 	// float controlValue = Mathf.Clamp((float)controlField.GetValue(moveShip) + mouseScroll, rangeMin, rangeMax);
		// 	// controlField.SetValue(moveShip, controlValue);
		// 	howActive += mouseScroll;
		// 	howActive = Mathf.Clamp(howActive, rangeMin, rangeMax);
		// 	transform.parent.localEulerAngles = new Vector3(howActive * 60, 0, 90);
		// }

		if (onHold) {
			Plane axlePlane = new Plane(transform.parent.up, hit.point);
			Vector3 localPoint = axlePlane.ClosestPointOnPlane(hit.point);
			float angle;
			// angle = Vector3.Angle(Vector3.up, localPoint - axlePlane.ClosestPointOnPlane(transform.parent.position));
			angle = Vector3.SignedAngle(Vector3.up, localPoint - axlePlane.ClosestPointOnPlane(transform.parent.position), -transform.up);
			Debug.Log(angle);
			howActive = Mathf.Clamp(angle/angleMax, rangeMin, rangeMax);
			transform.parent.localEulerAngles = new Vector3(howActive * angleMax, 0, 90);
		}

		// if (onClick) {
		// 	// controlField.SetValue(moveShip, 0);
		// 	howActive = 0;
		// 	transform.parent.localEulerAngles = new Vector3(howActive * 60, 0, 90);
		// }
	}

	// float GetAngle() {

	// }
}
