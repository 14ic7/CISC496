using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlashlightCollider : Cone {
	HashSet<Ghost> stunnedGhosts = new HashSet<Ghost>(); //add colliding ghosts to this hashset

	new Light light;

	void Start() {
		light = GetComponent<Light>();
	}

	// The light component in the parent flashlight is only enabled when the button is pressed and there is enough power.
	void OnTriggerEnter (Collider collider) {
		stunGhost(collider);	
	}
	void OnTriggerStay(Collider collider) {
		stunGhost(collider);
	}
	void stunGhost(Collider collider) {
		Ghost ghost = collider.GetComponentInParent<Ghost>();

		if (ghost != null) {
			if (RaycastGhost(ghost)) {
				ghost.stun();
				stunnedGhosts.Add(ghost);
			} else {
				ghost.unstun();
				stunnedGhosts.Remove(ghost);
			}
		}
	}

	void OnTriggerExit (Collider collider) {
		Debug.Log("Trigger exit!");
		Ghost ghost = collider.GetComponentInParent<Ghost>();
		if (ghost != null) {
			ghost.unstun ();
			stunnedGhosts.Remove (ghost);
		}
	}


	// --------------- PUBLIC FUNCTIONS ---------------

	public void turnOn() {
		gameObject.SetActive(true);
	}

	public void turnOff() {
		gameObject.SetActive(false);

		foreach (Ghost ghost in stunnedGhosts) {
			if (ghost != null) {
				ghost.unstun ();
			}
		}
		stunnedGhosts.Clear();
	}
}
