using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class LegacyPlayerMovement : MonoBehaviour {

#pragma warning disable 0649

	[SerializeField]
	private GameObject gfx;

	[SerializeField]
	private Bounds bounds;

#pragma warning restore 0649


	private Rigidbody2D rb;

	// input
	private float horizontalInput;
	private float verticalInput;
	private float rotationInput;
	private bool jumpInput;


	[SerializeField]
	private float boardSize = 1f;


	public float rotDEBUG;

	[Header("Speed:")]
	[SerializeField]
	private float constantSpeed = 1f;


	[Header("Movement:")]

	[Tooltip("How fast the player moves with the keyboard inputs.")]
	[SerializeField]
	private Vector2 keyMoveSpeed = new Vector2(1, 1);

	[Tooltip("The accelerations that currently apply on the rigitbody.")]
	public Vector3 acceleration = Vector3.zero;

	[Tooltip("The transformation the rigitbody made last physics interval.")]
	public Vector3 velocity = Vector2.zero;

	[Header("Vertical:")]

	[SerializeField]
	private float gravity = -1;

	[SerializeField]
	private float rocketStrength = 2f;


	[Header("Jumping:")]
	public bool canJump = true;
	private bool grounded = false;
	private bool jumping = false;

	[SerializeField]
	private float jumpAmplitude = 1;
	[SerializeField]
	private float jumpSpeed = 2;
	private float jumpTime = 0;



	[Header("Rotation")]

	[SerializeField]
	private float rotationSpeed = 3f;

	//[SerializeField]
	//private float rotationCorrectionSpeed = 0.5f;


	// ground variables
	private Obstacle collidingObs = null;
	private float groundHeight;
	private Vector3 groundRotation;



	// Obstacles
	// ---------

	private Vector2 additionalBoost = Vector2.zero;

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start() {
	}

	private void Update() {
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

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position - transform.right * boardSize, transform.position + transform.right * boardSize);
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


	private void FixedUpdate() {

		// calculate next position
		Vector3 newPos = Move();

		// set next position
		rb.MovePosition(new Vector3(newPos.x, newPos.y, 0));

		// calulcate rotation
		//float rotation = InvoluntaryRotation(newPos);
		float rotation = rb.rotation + rotationInput * rotationSpeed * Time.fixedDeltaTime;

		// set rotation
		rb.MoveRotation(rotation);



		rotDEBUG = rb.rotation;
	}

	/// <summary>
	/// returns the new Position or the transform
	/// </summary>
	private Vector3 Move() {
		Vector3 newPos = transform.position;

		// -------------- vertical movement -------------- //

		// jumping
		if (jumping == true) {
			acceleration.y += JumpingCurve(jumpTime);

			jumpTime += Time.fixedDeltaTime;

			if (jumpTime >= Mathf.PI * jumpSpeed || grounded == true)
				jumping = false;
		}

		// rocket thrust
		if (acceleration.y < 0)
			acceleration.y += (transform.right * rocketStrength).y;

		// gravity
		acceleration.y -= gravity;



		// -------------- horizontal movement -------------- //

		// constant speed
		newPos.x += constantSpeed;

		// input movement
		if (horizontalInput < 0)
			newPos.x = transform.position.x + keyMoveSpeed.x * horizontalInput;
		else if (horizontalInput > 0)
			newPos.x = transform.position.x + keyMoveSpeed.x * horizontalInput;

		// -------------- addtional movement -------------- //

		if (additionalBoost.sqrMagnitude != 0) {
			acceleration.y += additionalBoost.y;
			acceleration.x += additionalBoost.x;
			additionalBoost.y = 0;
		}

		// -------------- bounds -------------- //

		/*
		// horizontal bounds
		if (newPos.x < bounds.left)
			newPos.x = bounds.left;
		if (newPos.x > bounds.right)
			newPos.x = bounds.right;
		*/


		// vertical bounds
		groundHeight = bounds.height;
		groundRotation = Vector3.up;
		if (collidingObs != null && collidingObs.InBounds(transform.position.x)) {

			Obstacle.PointEvaluation obsGroundValues = collidingObs.Evaluate(transform.position.x);
			if (obsGroundValues.height > groundHeight) {
				groundHeight = obsGroundValues.height;
				groundRotation = obsGroundValues.normal;
			}
		}

		if (newPos.y <= groundHeight) {
			newPos.y = groundHeight;
			grounded = true;
			acceleration.y = 0;
		}



		// vertical movement
		newPos.y += acceleration.y;



		velocity = newPos - transform.position;
		return newPos;
	}

	/// <summary>
	/// returns the z rotation of the transform
	/// </summary>
	private float InvoluntaryRotation(Vector3 newPos) {


		// rotation
		transform.Rotate(gfx.transform.forward, rotationInput * rotationSpeed * Time.fixedDeltaTime);
		//transform.up = groundRotation;


		Vector3 newLeft = transform.position - transform.right * boardSize;
		Vector3 newRight = transform.position + transform.right * boardSize;

		if (newLeft.y < groundHeight) {

			newLeft.x -= newLeft.y - groundHeight;
			newLeft.y = groundHeight;
			//transform.right = newLeft - transform.position;
			Debug.DrawLine(transform.position, newLeft, Color.cyan);

			//newPos.y = groundHeight + newPos.y - newLeft.y;
			//transform.Rotate(gfx.transform.forward, -rotationCorrectionSpeed * Time.fixedDeltaTime);
		}
		if (newRight.y < groundHeight) {

			newRight.x -= newRight.y - groundHeight;
			newRight.y = groundHeight;
			//transform.right = newRight - transform.position;
			Debug.DrawLine(transform.position, newRight, Color.cyan);


			//newPos.y = groundHeight + newPos.y - newRight.y;
		}


		//Vector3.Angle(newRight - newPos, Vector3.right);

		//if (transform.up != groundRotation)
		//	transform.up = Vector3.Slerp(transform.up, groundRotation, rotationCorrectionSpeed);


		return 0;
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

	public void AddRocketBoost(float boost) {
		additionalBoost = transform.right * boost;
	}
}
