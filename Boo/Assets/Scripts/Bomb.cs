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
		vfx = this.transform.GetChild (0).gameObject;
		ps = vfx.GetComponent <ParticleSystem> ();
		donePriming = false;
		allDone = false;
		primeTime = new Timer (3.0f);
		primeTime.StartTimer ();
	}

	void Update () {
		if (!allDone) {
			if (primeTime.IsRunning ()) {
				primeTime.UpdateTimer ();
			} else if (!donePriming) {
				ps.Stop (true);
				vfx.GetComponentInChildren<Light> ().intensity -= 5.0f * Time.deltaTime;
				landmine = Instantiate (Resources.Load ("Landmine Indicator"), this.transform.position, Quaternion.identity) as GameObject;
				donePriming = true;
			} else {
				vfx.GetComponentInChildren<Light> ().intensity -= 5.0f * Time.deltaTime;
				if ((vfx.GetComponentInChildren<Light> ().intensity) <= 0.0f) {
					landmine.GetComponent<Light> ().intensity += 5.0f * Time.deltaTime;
					if (landmine.GetComponent<Light> ().intensity >= 1.0f) {
						allDone = true;
					}
				}
			}
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.tag == "Unit") {
			Rigidbody rb = collision.gameObject.GetComponent<Rigidbody> ();
			rb.GetComponent<Ghost> ().Damage (3.0f);
			if (rb != null) {
				sfx.Play ();
				StartCoroutine (CapsuleDelay (rb));
			}
		}
	}

	IEnumerator CapsuleDelay (Rigidbody rb) {
		if (rb != null) {
			rb.GetComponent<NavMeshAgent> ().enabled = false;
		}
		if (rb != null) {
			rb.GetComponent<AICharacterControl> ().enabled = false;
		}
		if (rb != null) {
			rb.GetComponent<ThirdPersonCharacter> ().enabled = false;
		}
		if (rb != null) {
			rb.isKinematic = false;
		}
		if (rb != null) {
			// rb.AddForce (transform.forward * 10.0f, ForceMode.Impulse);
			rb.AddExplosionForce(20.0f, this.transform.position, 1.0f, 2.0f, ForceMode.Impulse);
		}
		yield return new WaitForSeconds (1.0f);
		vfx.SetActive (false);
		this.GetComponent<SphereCollider> ().enabled = false;
		yield return new WaitForSeconds (2.0f);
		if (rb != null) {
			rb.transform.position = new Vector3 (rb.transform.position.x, 0, rb.transform.position.z);
		}
		if (rb != null) {
			rb.isKinematic = true;
		}
		if (rb != null) {
			rb.GetComponent<NavMeshAgent> ().enabled = true;
		}
		if (rb != null) {
			rb.GetComponent<AICharacterControl> ().enabled = true;
		}
		if (rb != null) {
			rb.GetComponent<ThirdPersonCharacter> ().enabled = true;
		}
		if (rb != null) {
			rb.GetComponent<AICharacterControl> ().SetDestination (rb.transform.position);
		}
		Destroy (this.gameObject);
	}

}
