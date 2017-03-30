using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour {

	new FlashlightCollider light;

	private float flashlightPower = 100.0f;
	private float flashlightDrain = 0.25f;

	private Image powerUI;

	// Use this for initialization
	void Start () {
		light = transform.GetChild(0).GetComponent<FlashlightCollider>();
		powerUI = GameObject.Find ("Power Remaining").GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger) || Input.GetKey (KeyCode.L)) && (flashlightPower > 0.0f)) {
			Debug.Log (flashlightPower);
			light.turnOn ();
			flashlightPower = Mathf.Clamp (flashlightPower - flashlightDrain, 0.0f, 100.0f);
			powerUI.fillAmount = flashlightPower / 100.0f;
		} else if ((OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger) || Input.GetKey (KeyCode.L)) && (flashlightPower <= 0.0f)) {
			light.turnOff ();
			flashlightPower = Mathf.Clamp (flashlightPower, 0.0f, 100.0f);
		} else {
			light.turnOff ();
		}
		if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) && (flashlightPower == 100.0f)) {
			LightExplosion();
		}
	}

	public void AddPower (float power) {
		flashlightPower = Mathf.Clamp(flashlightPower + power, 0.0f, 100.0f);
		powerUI.fillAmount = flashlightPower / 100.0f;
	}

	public void LightExplosion () {
		Collider[] colliders = Physics.OverlapSphere (transform.position, 10.0f);
		foreach (Collider hit in colliders) {
			if (hit.gameObject.name == "ghost-w") {
				Rigidbody rb = hit.GetComponentInParent<Rigidbody> ();
				Debug.Log (rb.gameObject.name);
				if (rb != null) {
					rb.GetComponent<CapsuleCollider> ().enabled = true;
					rb.AddExplosionForce (1000000000.0f, transform.position, 10.0f, 10.0f);
					rb.GetComponent<CapsuleCollider> ().enabled = false;
				}
			}
		}
	}

}
