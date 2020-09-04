using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looper : MonoBehaviour {

	public float xMax = 500f;

	public float xReset = 0f;

	public ParticleSystem[] particleSystems;
	public ParticleSystem.MainModule[] psMains;

	void Start() {
		psMains = new ParticleSystem.MainModule[particleSystems.Length];

		for (int i = 0; i < particleSystems.Length; i++) {
			psMains[i] = particleSystems[i].main;
		}
	}

	void Update() {
		if (transform.position.x > xMax) {
			Loop();
		}
	}

	private void Loop() {
		for (int i = 0; i < particleSystems.Length; i++) {
			psMains[i].simulationSpace = ParticleSystemSimulationSpace.Custom;
			psMains[i].customSimulationSpace = transform;
		}

		transform.position = new Vector3(0, transform.position.y, transform.position.z);

		for (int i = 0; i < particleSystems.Length; i++) {
			psMains[i].simulationSpace = ParticleSystemSimulationSpace.World;
			//psMains[i].customSimulationSpace = transform;
		}
	}
}
