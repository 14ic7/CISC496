using UnityEngine;
using System.Collections;

public class VacuumController : MonoBehaviour {

	new VacuumCollider vacuum;

	// Use this for initialization
	void Start () {
		vacuum = transform.GetChild (0).GetComponent<VacuumCollider> ();
		vacuum.turnOff ();
	}
	
	// Update is called once per frame
	void Update () {
		if (OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger) == true || Input.GetKey(KeyCode.F)) {
			vacuum.turnOn ();
		} else {
			vacuum.turnOff ();
		}
	}
		
}
