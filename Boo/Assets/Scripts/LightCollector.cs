using UnityEngine;
using System.Collections;

public class LightCollector : MonoBehaviour {

	GameObject flashlight;
	GameObject lightBall;
	GameObject bombBall;
	GameObject gun;

	private bool LightLerp;
	private bool BombLerp;

	public AudioSource sfx;

	AudioClip lightSFX;
	AudioClip bombSFX;

	void Start () {
		flashlight = GameObject.Find ("Flashlight");
		gun = GameObject.Find ("Gun");
		LightLerp = false;
		BombLerp = false;
		lightSFX = (AudioClip)Resources.Load ("Audio/sfx-light_pickup");
		bombSFX = (AudioClip)Resources.Load ("Audio/sfx-bombspawn");
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "LightBall") {
			lightBall = collider.gameObject;
			LightLerp = true;
		} else if (collider.tag == "BombBall") {
			bombBall = collider.gameObject;
			BombLerp = true;
		}
	}

	void Update () {
		if (LightLerp) {
			float step = 15.0f * Time.deltaTime;
			lightBall.transform.position = Vector3.MoveTowards (lightBall.transform.position, gun.transform.position, step);
			if (ApproxEqual (lightBall.transform.position, gun.transform.position, 3.0f)) {
				LightLerp = false;
				if (flashlight.GetComponent<FlashlightController> ().GetPower () >= 100.0f) {
					flashlight.GetComponent<FlashlightController> ().AddLightCharge ();
					// Add light blast count in UI form
				} else {
					flashlight.GetComponent<FlashlightController> ().AddPower (30);
				}
				sfx.PlayOneShot (lightSFX);
				Destroy (lightBall);
			}
		} else if (BombLerp) {
			float step = 15.0f * Time.deltaTime;
			bombBall.transform.position = Vector3.MoveTowards (bombBall.transform.position, gun.transform.position, step);
			if (ApproxEqual (bombBall.transform.position, gun.transform.position, 3.0f)) {
				BombLerp = false;
				GameObject.Find ("Vacuum").GetComponent<VacuumController> ().AddBomb ();
				sfx.PlayOneShot (bombSFX);
				Destroy (bombBall);
			}

		}
	}

	public static bool ApproxEqual (Vector3 v1, Vector3 v2, float precision) {
		bool equal = true;
		if (Mathf.Abs (v1.x - v2.x) > precision) {
			equal = false;
		}
		if (Mathf.Abs (v1.y - v2.y) > precision) {
			equal = false;
		}
		if (Mathf.Abs (v1.z - v2.z) > precision) {
			equal = false;
		}
		return equal;
	}

	public void turnOn () {
		gameObject.SetActive (true);
	}

	public void turnOff () {
		gameObject.SetActive (false);
	}
}
