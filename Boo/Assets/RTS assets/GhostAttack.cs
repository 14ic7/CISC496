using UnityEngine;
using System.Collections;

public class GhostAttack : MonoBehaviour {
	Ghost ghost;

	void Start() {
		ghost = transform.parent.GetComponent<Ghost>();
	}

	// called by a mecanim animation event
	public void hurtEnemy() {
		ghost.hurtEnemy();
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log("collision");
		// if collision is with ghost's targeted enemy
		if (ghost.enemy != null && collision.gameObject == ghost.enemy.gameObject) {
			ghost.Attack();
		}
	}
}
