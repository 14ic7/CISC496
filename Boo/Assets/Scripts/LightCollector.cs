using UnityEngine;
using System.Collections;

public class LightCollector : MonoBehaviour {

	GameObject flashlight;
	GameObject lightBall;
	GameObject gun;

	private bool lerp;

	public AudioSource sfx;

	void Start () {
		flashlight = GameObject.Find ("Flashlight");
		gun = GameObject.Find ("Gun");
		lerp = false;
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "LightBall") {
			lightBall = collider.gameObject;
			lerp = true;
		}
	}

	void Update () {
		if (lerp) {
			float step = 15.0f * Time.deltaTime;
			lightBall.transform.position = Vector3.MoveTowards (lightBall.transform.position, gun.transform.position, step);
			if (ApproxEqual(lightBall.transform.position, gun.transform.position, 2.0f)) {
				lerp = false;
				flashlight.GetComponent<FlashlightController> ().AddPower (30);
				Destroy (lightBall);
				sfx.Play ();
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
