using UnityEngine;
using System.Collections;

public class VacuumController : MonoBehaviour {

	new VacuumCollider vacuum;
	new LightCollector lc;

	GameObject vfx;

	// Use this for initialization
	void Start () {
		vacuum = transform.GetChild (0).GetComponent<VacuumCollider> ();
		lc = transform.GetChild (1).GetComponent<LightCollector> ();
		vfx = GameObject.Find ("Vacuum FX");
		vfx.SetActive (false);
		vacuum.turnOff ();
		lc.turnOff ();
	}
	
	// Update is called once per frame
	void Update () {
		if (OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger) || Input.GetKey(KeyCode.F)) {
			vacuum.turnOn ();
			lc.turnOn ();
			vfx.SetActive (true);
		} else {
			vacuum.turnOff ();
			lc.turnOff ();
			vfx.SetActive (false);
		}
	}
		
}
