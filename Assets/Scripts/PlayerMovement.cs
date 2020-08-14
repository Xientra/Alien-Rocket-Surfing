using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

	[SerializeField]
	private PlayerBounds playerBounds;

	[SerializeField]
	private float boundsMultiplier = 0.9f;

	[SerializeField]
	private Vector2 speed = new Vector2(1, 1);

	private float horizontalInput;
	private float verticalInput;

	private Rigidbody2D rb;
	private Camera cam;


	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start() {
		cam = Camera.main;
	}

	private void Update() {
		horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");

	}

	private void FixedUpdate() {

		float xBounds = playerBounds.scaleX * boundsMultiplier;
		float yBounds = playerBounds.scaleY * boundsMultiplier;

		float ySpeed = 0;
		float xSpeed = 0;

		if (horizontalInput < 0) {
			if (transform.position.x > -xBounds)
				xSpeed = speed.x * horizontalInput;
		}
		else if (horizontalInput > 0) {
			if (transform.position.x < xBounds)
				xSpeed = speed.x * horizontalInput;
		}

		if (verticalInput < 0) {
			if (transform.position.y > -yBounds)
				ySpeed = speed.y * verticalInput;
		}
		else if (verticalInput > 0) {
			if (transform.position.y < yBounds)
				ySpeed = speed.y * verticalInput;
		}

		rb.MovePosition(transform.position + new Vector3(xSpeed, ySpeed, 0));
	}
}
