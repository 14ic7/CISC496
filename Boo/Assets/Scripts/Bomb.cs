using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class Bomb : MonoBehaviour {

	AudioSource sfx;
	GameObject vfx;

	void Start () {
		sfx = GetComponent<AudioSource>();
		vfx = this.transform.GetChild (0).gameObject;
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.tag == "Unit") {
			Rigidbody rb = collision.gameObject.GetComponent<Rigidbody> ();
			rb.GetComponent<Ghost> ().Damage (5.0f);
			if (rb != null) {
				sfx.Play ();
				StartCoroutine (CapsuleDelay (rb));
			}
		}
	}

	IEnumerator CapsuleDelay (Rigidbody rb) {
		rb.GetComponent<NavMeshAgent> ().enabled = false;
		rb.GetComponent<AICharacterControl> ().enabled = false;
		rb.GetComponent<ThirdPersonCharacter> ().enabled = false;
		rb.isKinematic = false;
		rb.AddForce (transform.forward * 10.0f, ForceMode.Impulse);
		yield return new WaitForSeconds (1.0f);
		vfx.SetActive (false);
		this.GetComponent<SphereCollider> ().enabled = false;
		yield return new WaitForSeconds (2.0f);
		rb.transform.position = new Vector3 (rb.transform.position.x, 0, rb.transform.position.z);
		rb.isKinematic = true;
		rb.GetComponent<NavMeshAgent> ().enabled = true;
		rb.GetComponent<AICharacterControl> ().enabled = true;
		rb.GetComponent<ThirdPersonCharacter> ().enabled = true;
		rb.GetComponent<AICharacterControl> ().SetDestination (rb.transform.position);
		Destroy (this.gameObject);
	}

}
