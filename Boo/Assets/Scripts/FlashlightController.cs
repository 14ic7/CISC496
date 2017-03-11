using UnityEngine;
using System.Collections.Generic;

public class FlashlightController : MonoBehaviour {

	new FlashlightCollider light;

	private float flashlightPower = 100.0f;
	private float flashlightDrain = 0.5f;

	// Use this for initialization
	void Start () {
		light = transform.GetChild(0).GetComponent<FlashlightCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger) || Input.GetKey (KeyCode.L)) && (flashlightPower > 0.0f)) {
			Debug.Log (flashlightPower);
			light.turnOn ();
			flashlightPower = Mathf.Clamp (flashlightPower - flashlightDrain, 0.0f, 100.0f);
		} else if ((OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger) || Input.GetKey (KeyCode.L)) && (flashlightPower <= 0.0f)) {
			light.turnOff ();
			flashlightPower = Mathf.Clamp (flashlightPower, 0.0f, 100.0f);
		} else {
			light.turnOff ();
		}
	}

	public void AddPower (float power) {
		flashlightPower = Mathf.Clamp(flashlightPower + power, 0.0f, 100.0f);
	}

}
