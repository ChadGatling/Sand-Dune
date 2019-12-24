using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour {

	void Start () {
		// Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update () {		
		if (Input.GetButtonDown("Cancel")){
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#elif UNITY_WEBPLAYER
			Application.OpenURL(webplayerQuitURL);
			#else
			Application.Quit();
			#endif
		}
	}
}
