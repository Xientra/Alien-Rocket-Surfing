using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostGate : MonoBehaviour {

// #pragma warning disable 0169

	[SerializeField]
	private float boost = 1f;

	[SerializeField]
	private float rechargeTime = 1f;

	[SerializeField]
	private bool active = true;

// #pragma warning restore 0649

	private void OnTriggerEnter2D(Collider2D collision) {
		if (active == true) {
			if (collision.CompareTag("Player")) {
				PlayerMovement pm = collision.GetComponent<PlayerMovement>();

				pm.AddRocketBoost(boost);
				active = false;
				StartCoroutine(Recharge());
			}
		}
	}

	private IEnumerator Recharge() {
		yield return new WaitForSeconds(rechargeTime);
		active = true;
	}
}
