using UnityEngine;
using System.Collections;

public class VacuumController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (OVRInput.Get (OVRInput.RawButton.LIndexTrigger) == true) {
			Debug.Log ("vacuum is on");
		}

	}
}
