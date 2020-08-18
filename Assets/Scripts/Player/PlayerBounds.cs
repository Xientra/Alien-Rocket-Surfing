using UnityEngine;

public class PlayerBounds : MonoBehaviour {

	[SerializeField]
	private float boundsMultiplier = 0.9f;

	public Vector3 cornerTopLeft;
	public Vector3 cornerTopRight;
	public Vector3 cornerBottomLeft;
	public Vector3 cornerBottomRight;

	public float scaleX;
	public float scaleY;

	public float left;
	public float right;
	public float top;
	public float bottom;

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

		left = transform.position.x - scaleX;
		right = transform.position.x + scaleX;
		top = transform.position.y - scaleY;
		bottom = transform.position.y + scaleY;

		cornerTopLeft = transform.position + new Vector3(-scaleX, scaleY, 0);
		cornerTopRight = transform.position + new Vector3(scaleX, scaleY, 0);
		cornerBottomLeft = transform.position + new Vector3(-scaleX, -scaleY, 0);
		cornerBottomRight = transform.position + new Vector3(scaleX, -scaleY, 0);
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(cornerTopLeft, cornerBottomLeft);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(cornerTopRight, cornerBottomRight);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(cornerTopLeft, cornerTopRight);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(cornerBottomLeft, cornerBottomRight);
	}

	//private void OnValidate() {
	//}
}
