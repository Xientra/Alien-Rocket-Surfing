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
	private bool jumpInput;

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
		jumpInput = Input.GetAxis("Jump") != 0;
		if (jumpInput && jumping == false) {
			Jump();
		}
	}

	private void FixedUpdate() {
		float xPos = transform.position.x;
		float yPos = transform.position.y;

		float speedX = jumping ? speed.x / 2 : speed.x;

		// vertical movement
		if (jumping == true) {
			// jumping
			//gfx.transform.Rotate(gfx.transform.forward, -(360 / (Mathf.PI * jumpSpeed)) * Time.fixedDeltaTime);
			yPos = originalY + JumpingCurve(jumpTime);

			jumpTime += Time.fixedDeltaTime;

			if (jumpTime >=  Mathf.PI * (jumpSpeed))
				jumping = false;
		}
		else {
			// ground movement
			if (verticalInput < 0) {
				yPos = transform.position.y + speed.y * verticalInput;
				if (yPos < bounds.top)
					yPos = bounds.top;
			}
			else if (verticalInput > 0) {
				yPos = transform.position.y + speed.y * verticalInput;
				if (yPos > bounds.bottom)
					yPos = bounds.bottom;
			}
		}

		// horizontal movement
		if (horizontalInput < 0) {
			xPos = transform.position.x + speedX * horizontalInput;
			if (xPos < bounds.left)
				xPos = bounds.left;
		}
		else if (horizontalInput > 0) {
			xPos = transform.position.x + speedX * horizontalInput;
			if (xPos > bounds.right)
				xPos = bounds.right;
		}

		// set next position
		rb.MovePosition(new Vector3(xPos, yPos, 0));
	}

	public void Jump() {
		jumpTime = 0;
		jumping = true;
		originalY = transform.position.y;
	}

	private float JumpingCurve(float jumpingTime) {
		return Mathf.Sin(jumpTime * (1 / jumpSpeed)) * jumpAmplitude;
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.CompareTag("ParryCollider")) {
			if (jumpInput) {
				Jump();
			}
		}
	}
}
