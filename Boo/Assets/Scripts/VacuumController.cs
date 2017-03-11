using UnityEngine;
using System.Collections;

public class VacuumController : MonoBehaviour {

	new VacuumCollider vacuum;

	GameObject vfx;

	// Use this for initialization
	void Start () {
		vacuum = transform.GetChild (0).GetComponent<VacuumCollider> ();
		vfx = GameObject.Find ("Vacuum FX");
		vfx.SetActive (false);
		vacuum.turnOff ();
	}
	
	// Update is called once per frame
	void Update () {
		if (OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger) || Input.GetKey(KeyCode.F)) {
			vacuum.turnOn ();
			vfx.SetActive (true);
		} else {
			vacuum.turnOff ();
			vfx.SetActive (false);
		}
	}
		
}
