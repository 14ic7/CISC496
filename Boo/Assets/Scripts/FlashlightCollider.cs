using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlashlightCollider : MonoBehaviour {
	HashSet<Ghost> stunnedGhosts = new HashSet<Ghost>(); //add colliding ghosts to this hashset

	new Light light;

	void Start() {
		light = GetComponent<Light>();
	}

	// The light component in the parent flashlight is only enabled when the button is pressed and there is enough power.
	void OnTriggerEnter (Collider collider) {
		rayCastGhost(collider);	
	}
	void OnTriggerStay(Collider collider) {
		rayCastGhost(collider);
	}
	void rayCastGhost(Collider collider) {
		Ghost ghost = collider.GetComponentInParent<Ghost>();

		if (ghost != null) {
			//shoot a ray from the light to the ghost
			Ray ray = new Ray(light.transform.position, collider.transform.position - light.transform.position);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, light.range) && hit.transform == ghost.transform) {
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
