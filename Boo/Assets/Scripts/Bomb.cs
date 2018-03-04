using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class Bomb : MonoBehaviour {

	AudioSource sfx;
	GameObject vfx;
	Timer primeTime;
	ParticleSystem ps;
	bool donePriming;
	bool allDone;
	GameObject landmine;

	void Start () {
		sfx = GetComponent<AudioSource>();
		vfx = transform.GetChild(0).gameObject;
		ps = vfx.GetComponent<ParticleSystem>();
		donePriming = false;
		allDone = false;
		primeTime = new Timer(5.0f);
		primeTime.StartTimer();
	}

	void Update () {
		if (!allDone) {
			if (primeTime.IsRunning()) {
				primeTime.UpdateTimer();
			} else if (!donePriming) {
				ps.Stop (true);
				vfx.GetComponentInChildren<Light>().intensity -= 5.0f * Time.deltaTime;
				if (ApproxFloatEqual (transform.position.y, 0.0f, 1.0f)) {
					landmine = Instantiate (Resources.Load ("Landmine Indicator"), transform.position, Quaternion.Euler (-90.0f, 0.0f, 0.0f)) as GameObject;
					donePriming = true;
				} else {
					Destroy (gameObject);
				}
			} else {
				vfx.GetComponentInChildren<Light>().intensity -= 5.0f * Time.deltaTime;
				if ((vfx.GetComponentInChildren<Light>().intensity) <= 0.0f) {
					landmine.GetComponent<Light>().intensity += 5.0f * Time.deltaTime;
					if (landmine.GetComponent<Light>().intensity >= 1.0f) {
						allDone = true;
					}
				}
			}
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.tag == "Unit") {
			Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
			
			if (rb != null) {
				rb.GetComponent<Ghost>().Damage(10f);
				sfx.Play();
				Destroy(gameObject); // kill yourself
			}
		}
	}

	private static bool ApproxFloatEqual (float f1, float f2, float precision) {
		if (Mathf.Abs (f1 - f2) > precision) {
			return false;
		}
		
		return true;
	}

}
