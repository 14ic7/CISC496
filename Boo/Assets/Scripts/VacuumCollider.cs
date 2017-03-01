using UnityEngine;
using System.Collections;

public class VacuumCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter (Collider collider) {
		if ((transform.parent.GetComponent<VacuumController> ().isVacuumOn () == true) && (collider.tag == "Unit")) {
			// Implement ghost health system here, right now it just immediately destroys the ghost.
			Destroy (collider.gameObject);
		}
	}
}
