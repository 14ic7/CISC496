using UnityEngine;
using System.Collections;

public class VacuumController : MonoBehaviour {

	private bool isOn;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger) == true) {
			isOn = true;
		} else {
			isOn = false;
		}
	}

	public bool isVacuumOn () {
		return isOn;
	}
}
