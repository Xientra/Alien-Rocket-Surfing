using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	[SerializeField]
	private float moveSpeed = 0.5f;

	public Vector3[] points = new Vector3[0];

	void Start() {

	}

	void Update() {

	}

	public bool InBounds(float x) {
		return (x >= (transform.position + points[0]).x && x <= (transform.position + points[points.Length - 1]).x);
	}

	public float Evaluate(float x) {
		if (InBounds(x) == false)
			throw new ArgumentOutOfRangeException("x", "Call InBounds(float x) before calling Evaluate(float x)");
		
		for (int i = 1; i < points.Length; i++) {
			Vector3 p1 = points[i - 1] + transform.position;
			Vector3 p2 = points[i] + transform.position;

			if (x <= p2.x) {
				float t = 1 - (p2.x - x) / (p2.x - p1.x);
				//Debug.Log("r: " + r + " i: " + i + " t: " + t);

				return Vector3.Lerp(p1, p2, t).y;
			}
		}

		throw new ArgumentOutOfRangeException("x", "x was out of bounds, call InBounds(float x) before calling Evaluate(float x)");
	}

	private void FixedUpdate() {
		transform.Translate(-transform.right * moveSpeed);
	}

	private void OnDrawGizmos() {

		for (int i = 1; i < points.Length; i++) { 
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(transform.position + points[i - 1], transform.position + points[i]);
		}
	}
}
