using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class FlashlightController : MonoBehaviour {

	new FlashlightCollider light;

	private float flashlightPower = 25.0f;
	private float flashlightDrain = 0.25f;

	private Image powerUI;

	public ParticleSystem pulse;
	public ParticleSystem aura;
	public ParticleSystem dust;

	Timer capsuleDelay;
	GameObject blastReadyVFX;
	AudioClip triggerSFX;
	AudioClip blastSFX;

	// Use this for initialization
	void Start () {
		blastReadyVFX = GameObject.Find ("Light Ready");
		blastReadyVFX.SetActive (false);
		triggerSFX = (AudioClip)Resources.Load ("Audio/sfx-lightswitch");
		blastSFX = (AudioClip)Resources.Load ("Audio/Light-BlastFX");
		light = transform.GetChild(0).GetComponent<FlashlightCollider>();
		powerUI = GameObject.Find ("Power Remaining").GetComponent<Image> ();
		powerUI.fillAmount = flashlightPower / 100.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if ((OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger) || Input.GetKey (KeyCode.L)) && (flashlightPower > 0.0f)) {
			GetComponent<AudioSource> ().Play ();
			light.turnOn ();
			flashlightPower = Mathf.Clamp (flashlightPower - flashlightDrain, 0.0f, 100.0f);
			powerUI.fillAmount = flashlightPower / 100.0f;
		} else if ((OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger) || Input.GetKey (KeyCode.L)) && (flashlightPower <= 0.0f)) {
			light.turnOff ();
			flashlightPower = Mathf.Clamp (flashlightPower, 0.0f, 100.0f);
		} else {
			light.turnOff ();
		}
		if (flashlightPower == 100.0f) {
			powerUI.color = new Color (1, 0.92f, 0.016f, 1);
			blastReadyVFX.SetActive (true);
		}
		if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) && (flashlightPower == 100.0f)) {
			aura.Play ();
			pulse.Play ();
			dust.Play ();
			GetComponent<AudioSource> ().PlayOneShot (blastSFX, 0.5f);
			LightExplosion();
			flashlightPower = 0.0f;
			powerUI.fillAmount = flashlightPower / 100.0f;
			powerUI.color = new Color (1, 1, 1, 1);
			blastReadyVFX.SetActive (false);
		}
	}

	public void AddPower (float power) {
		flashlightPower = Mathf.Clamp(flashlightPower + power, 0.0f, 100.0f);
		powerUI.fillAmount = flashlightPower / 100.0f;
	}

	public void LightExplosion () {
		
		Collider[] colliders = Physics.OverlapSphere (transform.position, 10.0f);
		foreach (Collider hit in colliders) {
			if (hit.gameObject.tag == "Unit") {
				Rigidbody rb = hit.GetComponent<Rigidbody> ();
				if (rb != null) {
					StartCoroutine(CapsuleDelay(rb));
				}
			}
		}
	}

	IEnumerator CapsuleDelay (Rigidbody rb) {
		rb.GetComponent<NavMeshAgent> ().enabled = false;
		rb.GetComponent<AICharacterControl> ().enabled = false;
		rb.GetComponent<ThirdPersonCharacter> ().enabled = false;
		rb.isKinematic = false;
		rb.AddExplosionForce (150.0f, Vector3.zero, 10.0f, 1.0f);
		yield return new WaitForSeconds (3.0f);
		rb.transform.position = new Vector3 (rb.transform.position.x, 0, rb.transform.position.z);
		rb.isKinematic = true;
		rb.GetComponent<NavMeshAgent> ().enabled = true;
		rb.GetComponent<AICharacterControl> ().enabled = true;
		rb.GetComponent<ThirdPersonCharacter> ().enabled = true;
		rb.GetComponent<AICharacterControl> ().SetDestination (rb.transform.position);
	}

}
