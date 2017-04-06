using UnityEngine;
using System.Collections;

public class VacuumCollider : MonoBehaviour {

	private Timer damageDelay;

	// Use this for initialization
	void Start () {
		damageDelay = new Timer (0.5f);
	}
	
	void OnTriggerEnter (Collider collider) {
		// Damage the ghost. Deaths should be handled in the Ghost script to avoid NullReferenceExceptions.
		damageGhost(collider);
	}

	void OnTriggerStay (Collider collider) {
		// Continuously damage the ghost as long as they are in the vacuum.
		damageGhost(collider);		
	}

	void damageGhost(Collider collider) {
		Ghost ghost = collider.GetComponentInParent<Ghost>();
		if (ghost != null) {
			ghost.Damage (2.4f * Time.fixedDeltaTime);
			GetComponent<AudioSource> ().Play ();
		} else {
			GetComponent<AudioSource> ().Stop ();
		}
	}

	public void turnOn() {
		gameObject.SetActive (true);
	}

	public void turnOff() {
		gameObject.SetActive (false);
	}
		
	void Update () {
		if (damageDelay.IsRunning () == true) {
			damageDelay.UpdateTimer ();
		}

		transform.Rotate(new Vector3(0, 0, 180 * Time.deltaTime));
	}
}
	