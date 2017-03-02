using UnityEngine;
using System.Collections;

public class FlashlightController : MonoBehaviour {

	public Light flashlight;

	// Use this for initialization
	void Start () {
		flashlight = gameObject.GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger) == true) {
			Debug.Log ("flashlight is on");
			flashlight.enabled = true;
		} else if (OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger) == false) {
			flashlight.enabled = false;
		}
	
	}
		
}
