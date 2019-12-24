using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : ObjectAction {

	void Update() {

	}

	public override void Action() {
		isActive = Input.GetButton("Action");
	}
}
