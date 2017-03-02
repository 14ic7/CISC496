using UnityEngine;
using System.Collections.Generic;

public class FlashlightController : MonoBehaviour {

	new FlashlightCollider light;

	// Use this for initialization
	void Start () {
		light = transform.GetChild(0).GetComponent<FlashlightCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)) {
			Debug.Log ("flashlight is on");
			light.turnOn();
		} else {
			light.turnOff();
		}
	}
}
