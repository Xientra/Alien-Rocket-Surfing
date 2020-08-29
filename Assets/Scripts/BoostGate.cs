using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostGate : MonoBehaviour {

	[SerializeField]
	private float boost = 1f;

	[SerializeField]
	private bool active = true;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (active == true) {
			if (collision.CompareTag("Player")) {
				PlayerMovement pm = collision.GetComponent<PlayerMovement>();

				pm.AddRocketBoost(boost);
				active = false;
			}
		}
	}
}
