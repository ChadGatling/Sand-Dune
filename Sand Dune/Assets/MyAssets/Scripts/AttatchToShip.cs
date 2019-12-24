using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttatchToShip : MonoBehaviour {

	[SerializeField]GameObject player;

	void OnTriggerEnter(Collider col) {
		if (col.gameObject == player) {
			player.GetComponent<PlayerMove>().hasParent = true;
			player.transform.parent = transform;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject == player)
			player.GetComponent<PlayerMove>().hasParent = false;
			player.transform.parent = null;
	}
}
