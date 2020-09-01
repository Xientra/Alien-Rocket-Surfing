using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	
	public GameObject target;

#pragma warning disable 0414

	[Tooltip("How far behind this wants to be behind the target.")]
	[SerializeField]
	private float dradDistance = 0.5f;

	[SerializeField]
	private float lerpSmoothness = 0.5f;

#pragma warning restore 0414

	private Vector3 targetOffset;

	private void Start() {
		targetOffset = transform.position - target.transform.position;
	}

	void LateUpdate() {

		// bc i am in update now i need to multiply with Time.deltaTime somewhere

		//transform.position = Vector3.Lerp(transform.position, target.transform.position + targetOffset, (lerpSmoothness * 200) * Time.deltaTime); // 0.0166
		//transform.position = Vector3.Lerp(transform.position, target.transform.position + targetOffset, lerpSmoothness); // 0.0166

		Vector3 newPos = transform.position;

		newPos.x = (target.transform.position + targetOffset).x;
		//newPos.y = Vector3.Lerp(transform.position, target.transform.position + targetOffset, lerpSmoothness).y;
		transform.position = newPos;


		/*
		Vector3 idealPosition = target.transform.position + targetOffset + -target.velocity * target.velocity.magnitude * dradDistance;

		Debug.DrawLine(target.transform.position, idealPosition - targetOffset, Color.cyan);

		transform.position = Vector3.Lerp(transform.position, idealPosition, lerpSmoothness);
		*/
	}
}
