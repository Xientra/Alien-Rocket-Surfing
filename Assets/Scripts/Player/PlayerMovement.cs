using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

	[SerializeField]
	private GameObject gfx;

	[SerializeField]
	private PlayerBounds bounds;

	private Rigidbody2D rb;

	// input
	private float horizontalInput;
	private float verticalInput;
	private float rotationInput;
	private bool jumpInput;



	[SerializeField]
	private Vector2 speed = new Vector2(1, 1);
	[Tooltip("In angle per second")]
	[SerializeField]
	private float rotationSpeed = 3f;

	[Header("Vertical:")]

	[SerializeField]
	private float gravity = -1;


	[SerializeField]
	private float rocketStrength = 2f;

	[Header("Jumping:")]
	public bool canJump = true;

	private bool grounded = false;

	[SerializeField]
	private float jumpAmplitude = 1;
	[SerializeField]
	private float jumpSpeed = 2;
	private bool jumping = false;
	private float jumpTime = 0;



	// ground variables
	private Obstacle collidingObs = null;
	private float groundHeight;
	private float groundRotation;


	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start() {
	}

	private void Update() {
		horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");
		rotationInput = Input.GetAxisRaw("Rotation");
		jumpInput = Input.GetAxis("Jump") != 0;
		if (jumpInput && jumping == false && grounded == true) {
			Jump();
		}
	}

	private void FixedUpdate() {

		Vector3 newPos = transform.position;

		float speedX = !grounded ? speed.x / 2 : speed.x;

		// jumping
		if (jumping == true) {
			newPos.y += JumpingCurve(jumpTime);

			jumpTime += Time.fixedDeltaTime;

			if (jumpTime >= Mathf.PI * jumpSpeed || grounded == true)
				jumping = false;
		}

		// gravity
		newPos.y -= gravity;

		// ground collision
		groundHeight = bounds.height;
		if (collidingObs != null && collidingObs.InBounds(transform.position.x)) {
			float obsHeight = collidingObs.Evaluate(transform.position.x);
			if (obsHeight > groundHeight)
				groundHeight = obsHeight;
		}

		if (newPos.y <= groundHeight) {
			newPos.y = groundHeight;
			grounded = true;
		}


		// horizontal movement
		if (horizontalInput < 0)
			newPos.x = transform.position.x + speedX * horizontalInput;
		else if (horizontalInput > 0)
			newPos.x = transform.position.x + speedX * horizontalInput;

		// rotation
		transform.Rotate(gfx.transform.forward, rotationInput * rotationSpeed * Time.fixedDeltaTime);

		// rocket thrust
		newPos.y += (transform.right * rocketStrength).y;


		// horizontal bounds
		if (newPos.x < bounds.left)
			newPos.x = bounds.left;
		if (newPos.x > bounds.right)
			newPos.x = bounds.right;


		// set next position
		rb.MovePosition(new Vector3(newPos.x, newPos.y, 0));
	}

	public void Jump() {
		if (canJump) {
			jumpTime = 0;
			jumping = true;
			grounded = false;
		}
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

	private void OnTriggerEnter2D(Collider2D collision) {
		Obstacle obs = collision.GetComponent<Obstacle>();
		if (obs != null)
			collidingObs = obs;
	}

	private void OnTriggerExit2D(Collider2D collision) {
		Obstacle obs = collision.GetComponent<Obstacle>();
		if (obs == collidingObs)
			collidingObs = null;
	}
}
