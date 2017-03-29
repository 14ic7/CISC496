using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlashlightCollider : MonoBehaviour {

	HashSet<Ghost> stunnedGhosts = new HashSet<Ghost>(); //add colliding ghosts to this hashset

	void OnTriggerEnter (Collider collider) {
		// The light component in the parent flashlight is only enabled when the button is pressed and there is enough power.
		Debug.Log(collider.name+" enter/stay!");

		Ghost ghost = collider.GetComponentInParent<Ghost>();
		if (ghost == null) {
			return;
		}

		// Determine whether the ghost is looking into the light (0 degrees) or away from the light (> 190 degrees)
		// float angle = Vector3.Angle(light.transform.position - ghost.transform.position, ghost.transform.forward);

		if (true /*angle > 190*/) {  // Front or back is hit
			ghost.stun(); // stun the ghost immediately
			stunnedGhosts.Add(ghost);
		} else {
			ghost.unstun();
			stunnedGhosts.Remove(ghost);
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
