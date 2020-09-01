using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HorizontalMovement : MonoBehaviour {

	private Rigidbody2D rb;

	[SerializeField]
	private float constantSpeed = 0.5f;

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate() {

		Vector3 newPos = transform.position;

		// constant speed
		newPos.x += constantSpeed;

		rb.MovePosition(newPos);
	}
}
