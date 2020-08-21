using UnityEngine;

public class PlayerBounds : MonoBehaviour {

	[SerializeField]
	private float boundsMultiplier = 0.9f;

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

	private void UpdateValues() {
		scaleX = (transform.localScale.x / 2) * boundsMultiplier;
		scaleY = (transform.localScale.y / 2) * boundsMultiplier;

		height = transform.position.y;

		left = transform.position.x - scaleX;
		right = transform.position.x + scaleX;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position + new Vector3(scaleX, 0, 0), transform.position + new Vector3(-scaleX, 0, 0));
	}
}
