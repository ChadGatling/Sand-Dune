using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullAction : ObjectAction {

	void Start () {
		
	}
	
	void Update () {
		
	}

	public override void Action() {
		bool onClick = Input.GetButtonDown("Action");
		
		if (onClick) {
            isActive = !isActive;
			Vector3 move = new Vector3(0, isActive ? .25f : -.25f, 0);
			transform.Translate(move);
		}
	}
}
