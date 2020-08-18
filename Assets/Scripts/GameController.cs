using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController activeInstance;

	public Bullet bulletPrefab;

	public List<Bullet> bullets;

	private void Awake() {
		if (activeInstance == null)
			activeInstance = this;
	}

	void Start() {
		
	}

	void Update() {
		
	}
}
