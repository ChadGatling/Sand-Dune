using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour {

	ParticleSystem precipitation;

	void Start () {
		precipitation = GetComponent<ParticleSystem>();
	}
	
	void Update () {
        ParticleSystem.EmissionModule emission = precipitation.emission;
		emission.rateOverTime = Time.timeSinceLevelLoad;
	}
}
