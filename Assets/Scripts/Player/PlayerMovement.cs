using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

	[SerializeField]
	private GameObject gfx;

	[SerializeField]
	private PlayerBounds bounds;

	private Rigidbody2D rb;




	[SerializeField]
	private Vector2 speed = new Vector2(1, 1);
	[Tooltip("In angle per second")]
	[SerializeField]
	private float rotationSpeed = 3f;

	[SerializeField]
	private float gravity = -1;

	private bool grounded = false;


	private float horizontalInput;
	private float verticalInput;
	private float rotationInput;
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
		rotationInput = Input.GetAxisRaw("Rotation");
		Debug.Log(rotationInput);
		jumpInput = Input.GetAxis("Jump") != 0;
		if (jumpInput && jumping == false && grounded == true) {
			Jump();
		}
	}

	private void FixedUpdate() {
		float xPos = transform.position.x;
		float yPos = transform.position.y;

		float speedX = !grounded ? speed.x / 2 : speed.x;

		// vertical movement
		if (jumping == true) {
			// jumping
			yPos += JumpingCurve(jumpTime);

			jumpTime += Time.fixedDeltaTime;

			if (jumpTime >= Mathf.PI * jumpSpeed || grounded == true)
				jumping = false;
		}

		// gravity
		yPos -= gravity;
		if (yPos <= bounds.height) {
			yPos = bounds.height;
			grounded = true;
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

		// rotation
		transform.Rotate(gfx.transform.forward, rotationInput * rotationSpeed * Time.fixedDeltaTime);

		// set next position
		rb.MovePosition(new Vector3(xPos, yPos, 0));
	}

	public void Jump() {
		jumpTime = 0;
		jumping = true;
		originalY = transform.position.y;
		grounded = false;
	}

	private float JumpingCurve(float jumpingTime) {
		return (Mathf.Cos(jumpTime * (1 / jumpSpeed)) + 1) * jumpAmplitude;
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.CompareTag("ParryCollider")) {
			if (jumpInput) {
				Jump();
			}
		}
	}
}
