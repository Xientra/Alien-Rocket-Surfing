using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	[SerializeField]
	private float moveSpeed = 0.5f;

	public Vector3[] points = new Vector3[0];

	[Header("Gizmos:")]
	public bool showNormals = true;

	void Start() {

	}

	void Update() {

	}

	public bool InBounds(float x) {
		return (x >= (transform.position + points[0]).x && x <= (transform.position + points[points.Length - 1]).x);
	}

	public PointEvaluation Evaluate(float x) {
		if (InBounds(x) == false)
			throw new ArgumentOutOfRangeException("x", "Call InBounds(float x) before calling Evaluate(float x)");

		PointEvaluation result;

		for (int i = 1; i < points.Length; i++) {
			Vector3 p1 = points[i - 1] + transform.position;
			Vector3 p2 = points[i] + transform.position;

			if (x <= p2.x) {
				float t = 1 - (p2.x - x) / (p2.x - p1.x);

				float height = Vector3.Lerp(p1, p2, t).y;
				result = new PointEvaluation(height, CalculateNormal2D(p1, p2));

				//Debug.Log("h: " + height + ", r: " + rotation + ", i: " + i + ", t: " + t);
				return result;
			}
		}

		throw new ArgumentOutOfRangeException("x", "x was out of bounds. Call InBounds(float x) before calling Evaluate(float x)");
	}

	private void FixedUpdate() {
		transform.Translate(-transform.right * moveSpeed);
	}

	private void OnDrawGizmos() {

		for (int i = 1; i < points.Length; i++) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(transform.position + points[i - 1], transform.position + points[i]);

			if (showNormals) {
				Vector3 midPoint = transform.position + Vector3.Lerp(points[i - 1], points[i], 0.5f);

				Gizmos.color = Color.green;
				Gizmos.DrawLine(midPoint, midPoint + CalculateNormal2D(points[i - 1], points[i]));
			}
		}
	}

	private Vector3 CalculateNormal2D(Vector3 p1, Vector3 p2) {
		Vector3 direction = p1.x < p2.x ? p2 - p1 : p1 - p2;
		Vector3 normal = new Vector3(-direction.y, direction.x, direction.z);

		return normal.normalized;
	}


	public struct PointEvaluation {
		public readonly float height;
		public readonly Vector3 normal;

		public PointEvaluation(float height, Vector3 normal) : this() {
			this.height = height;
			this.normal = normal;
		}
	}

}
