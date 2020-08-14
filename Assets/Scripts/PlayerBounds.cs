using UnityEngine;

public class PlayerBounds : MonoBehaviour {

	public Vector3 cornerTopLeft;
	public Vector3 cornerTopRight;
	public Vector3 cornerBottomLeft;
	public Vector3 cornerBottomRight;

	public float scaleX;
	public float scaleY;

	public float boundX;
	public float boundY;

	private void OnDrawGizmos() {
		scaleX = transform.localScale.x / 2;
		scaleY = transform.localScale.y / 2;

		//boundX = transform.position.x + 

		cornerTopLeft = transform.position + new Vector3(-scaleX, scaleY, 0);
		cornerTopRight = transform.position + new Vector3(scaleX, scaleY, 0);
		cornerBottomLeft = transform.position + new Vector3(-scaleX, -scaleY, 0);
		cornerBottomRight = transform.position + new Vector3(scaleX, -scaleY, 0);

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
