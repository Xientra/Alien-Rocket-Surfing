using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement2 : MonoBehaviour {

	private Rigidbody2D rb;

#pragma warning disable 0649

	[SerializeField]
	private GameObject gfx;

#pragma warning restore 0649

	// input
	private float horizontalInput;
	private float verticalInput;
	private float rotationInput;
	private bool jumpInput;


	[Header("General:")]
	[SerializeField]
	private float boardSize = 1f;

	public float groundHeight = -4f;



	[Header("Movement:")]
	[SerializeField]
	private float constantSpeed = 1f;

	[SerializeField]
	private float inputMoveSpeed = 1;

	public Vector3 acceleration = Vector3.zero;
	public Vector3 velocity = Vector2.zero;

	[Space(5)]

	[SerializeField]
	private float gravity = -1;

	[SerializeField]
	private float accelerationDrag = -1;

	[SerializeField]
	private float rocketStrength = 2f;



	[Header("Jumping:")]
	private bool grounded = false;
	private bool jumping = false;
	public float jumpPower = 2f;



	[Header("Rotation:")]

	[SerializeField]
	private float rotationSpeed = 3f;



	private Vector2 additionalBoost = Vector2.zero;




	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	void Update() {
		GetInput();
	}

	private void GetInput() {
		horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");
		rotationInput = Input.GetAxisRaw("Rotation");
		jumpInput = Input.GetAxis("Jump") != 0;
		if (jumpInput && jumping == false && grounded == true) {
			Jump();
		}
	}


	private void FixedUpdate() {

		// calculate next position
		Vector3 newPos = Move();

		// set next position
		rb.MovePosition(new Vector3(newPos.x, newPos.y, 0));

		// calulcate rotation
		float rotation = rb.rotation + rotationInput * rotationSpeed * Time.fixedDeltaTime;

		// set rotation
		rb.MoveRotation(rotation);
	}

	private Vector3 Move() {
		Vector3 newPos = transform.position;


		// -------------- input horizontal movement -------------- //

		// input movement
		if (horizontalInput < 0)
			newPos.x = transform.position.x + inputMoveSpeed * horizontalInput;
		else if (horizontalInput > 0)
			newPos.x = transform.position.x + inputMoveSpeed * horizontalInput;


		// -------------- accerleration -------------- //

		// rocket thrust
		if (acceleration.y < 0)
			acceleration.y += (transform.right * rocketStrength).y;

		// gravity
		acceleration.y -= gravity;


		if (additionalBoost.sqrMagnitude != 0) {
			acceleration.y += additionalBoost.y;
			acceleration.x += additionalBoost.x;
			additionalBoost = Vector3.zero;
		}

		// acceleration decrease
		if (acceleration.x > 0) {
			acceleration.x -= accelerationDrag;
			acceleration.x = acceleration.x < 0 ? 0 : acceleration.x;
		} else {
			acceleration.x += accelerationDrag;
			acceleration.x = acceleration.x > 0 ? 0 : acceleration.x;
		}
		//float accMag = acceleration.magnitude - accelerationDrag;
		//acceleration = accMag > 0 ? acceleration.normalized * accMag : Vector3.zero;


		// vertical movement
		newPos += acceleration;

		// -------------- bounds -------------- //

		// vertical bounds
		if (newPos.y <= groundHeight) {
			newPos.y = groundHeight;
			grounded = true;
			jumping = false;
			acceleration.y = 0;
		}


		velocity = newPos - transform.position;
		return newPos;
	}


	public void Jump() {
		jumping = true;
		grounded = false;
		acceleration.y += jumpPower;
	}

	public void AddRocketBoost(float boost) {
		additionalBoost = transform.right * boost;
	}
}
