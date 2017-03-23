using UnityEngine;
using System.Collections;

public class GhostAttack : MonoBehaviour {
	PlayerHealth player;
	Ghost ghost;

	void Start() {
		ghost = transform.parent.GetComponent<Ghost>();
		player = FindObjectOfType<PlayerHealth>();
	}

	public void hurtPlayer() {
		player.Damage(10);
	}

	void OnCollisionEnter(Collision collision) {
		ghost.Attack(collision);
	}
}
