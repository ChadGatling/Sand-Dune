using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : ObjectAction {

	void Update() {

	}

	public override void Action(RaycastHit hit) {
		isActive = Input.GetButton("Action");
	}
}
