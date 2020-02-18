using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldClick : MonoBehaviour {

	[SerializeField] float raylength;
	
	private LayerMask LayerMask;

	void Start () {
		LayerMask = LayerMask.GetMask("Action" ,"Default");		
	}
	
	void Update () {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		bool raycast = Physics.Raycast(ray, out hit, raylength, LayerMask);

		if (raycast && hit.collider.tag == "Interactable") {
			hit.collider.GetComponent<ObjectAction>().Action(hit);
		}
	}
}
