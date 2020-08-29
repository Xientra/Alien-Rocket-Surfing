using UnityEngine;

public class Bounds : MonoBehaviour {

	[SerializeField]
	private float boundsMultiplier = 0.9f;

	public bool drawGizmos;

	public float scaleX;
	public float scaleY;

	public float height;

	public float left;
	public float right;

	private void Awake() {
		UpdateValues();
	}

	private void Update() {
		if (transform.hasChanged) {
			UpdateValues();
		}
	}

	public void UpdateValues() {
		scaleX = (transform.localScale.x / 2) * boundsMultiplier;
		scaleY = (transform.localScale.y / 2) * boundsMultiplier;

		height = transform.position.y;

		left = transform.position.x - scaleX;
		right = transform.position.x + scaleX;
	}

	private void OnDrawGizmos() {
		if (drawGizmos) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(transform.position + new Vector3(scaleX, 0, 0), transform.position + new Vector3(-scaleX, 0, 0));

		}
	}

	private void OnValidate() {
		UpdateValues();
	}
}
