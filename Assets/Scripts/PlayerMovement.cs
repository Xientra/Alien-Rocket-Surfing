using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

	[SerializeField]
	private GameObject gfx;

	[SerializeField]
	private PlayerBounds bounds;

	[SerializeField]
	private Vector2 speed = new Vector2(1, 1);

	private Rigidbody2D rb;



	private float horizontalInput;
	private float verticalInput;
	private float jumpInput;

	[SerializeField]
	private float jumpAmplitude = 1;
	[SerializeField]
	private float jumpSpeed = 2;
	private bool jumping = false;
	private float jumpTime = 0;
	private float originalY;



	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start() {
	}

	private void Update() {
		horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");
		if (Input.GetAxis("Jump") == 1 && jumping == false) {
			jumpTime = 0;
			jumping = true;
			originalY = transform.position.y;
		}
	}

	private void FixedUpdate() {
		float xPos = transform.position.x;
		float yPos = transform.position.y;

		float speedX = jumping ? speed.x / 2 : speed.x;

		// vertical movement
		if (jumping == true) {
			// jumping
			gfx.transform.Rotate(gfx.transform.forward, -(360 / (Mathf.PI * jumpSpeed)) * Time.fixedDeltaTime);
			yPos = originalY + JumpingCurve(jumpTime);

			jumpTime += Time.fixedDeltaTime;

			if (jumpTime >=  Mathf.PI * (jumpSpeed))
				jumping = false;
		}
		else {
			// ground movement
			if (verticalInput < 0) {
				if (transform.position.y > bounds.top)
					yPos = transform.position.y + speed.y * verticalInput;
			}
			else if (verticalInput > 0) {
				if (transform.position.y < bounds.bottom)
					yPos = transform.position.y + speed.y * verticalInput;
			}

		}

		// horizontal movement
		if (horizontalInput < 0) {
			if (transform.position.x > bounds.left)
				xPos = transform.position.x + speedX * horizontalInput;
		}
		else if (horizontalInput > 0) {
			if (transform.position.x < bounds.right)
				xPos = transform.position.x + speedX * horizontalInput;
		}

		// set next position
		rb.MovePosition(new Vector3(xPos, yPos, 0));
	}

	private float JumpingCurve(float jumpingTime) {
		return Mathf.Sin(jumpTime * (1 / jumpSpeed)) * jumpAmplitude;
	}
}
