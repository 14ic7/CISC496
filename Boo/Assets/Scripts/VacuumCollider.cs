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
		if ((damageDelay.IsRunning () == false) && (damageDelay.IsFirstCycle () == false)) {
			Debug.Log (collider.name);
			collider.GetComponentInParent<Ghost> ().hurt (2);
			damageDelay.ResetTimer ();
		} else if ((damageDelay.IsRunning () == false) && (damageDelay.IsFirstCycle () == true)) {
			Debug.Log (collider.name);
			collider.GetComponentInParent<Ghost> ().hurt (2);
			damageDelay.StartTimer ();
		}
	}

	void OnTriggerStay (Collider collider) {
		// Continuously damage the ghost as long as they are in the vacuum. Has a short delay between damage ticks.
		if ((damageDelay.IsRunning () == false) && (damageDelay.IsFirstCycle () == false)) {
			Debug.Log (collider.name);
			collider.GetComponentInParent<Ghost> ().hurt (2);
			damageDelay.ResetTimer ();
		} else if ((damageDelay.IsRunning () == false) && (damageDelay.IsFirstCycle () == true)) {
			Debug.Log (collider.name);
			collider.GetComponentInParent<Ghost> ().hurt (2);
			damageDelay.StartTimer ();
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
	}
}
	