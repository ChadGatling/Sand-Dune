using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {
	public float mouseSensitivity;
	public Transform playerBody;

	private float mouseX = 0f, mouseY = 0f;
	public bool inspectView = false;
	
	private void Update() {
		CameraRotation();
		LookMode();
	}

	private void LookMode() {
		if (inspectView && (Mathf.Abs(Input.GetAxis("Vertical")) >= .9f || Mathf.Abs(Input.GetAxis("Horizontal"))  >= .9f)) {
			inspectView = false;
		}

		if (Input.GetButtonDown("Look Mode")) {
			inspectView = !inspectView;
		}
	}

	private void CameraRotation() {
		// if (Input.GetButton("Build"))
		// 	return;

		if (inspectView) {
			Cursor.lockState = CursorLockMode.None;
		} else {
			mouseX += Input.GetAxis("Mouse X");
			mouseY += -Input.GetAxis("Mouse Y");
			mouseY = Mathf.Clamp(mouseY, -90f, 90f);

			Cursor.lockState = CursorLockMode.Locked; // seems take up a good amount of cpu time. Replace this with a UI reticle.
			transform.eulerAngles = new Vector3(mouseY, transform.eulerAngles.y, 0);
		}
		
		playerBody.eulerAngles = new Vector3(0, mouseX, 0); // I think this is the one that needs to be active for the player to ride on the ship

		#if UNITY_EDITOR
		Cursor.visible = true;
		#else
		Cursor.visible = false;
		#endif
	}
}
