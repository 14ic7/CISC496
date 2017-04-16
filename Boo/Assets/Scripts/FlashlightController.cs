using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class FlashlightController : MonoBehaviour {

	new FlashlightCollider light;

	private float flashlightPower = 100.0f;
	private float flashlightDrain = 0.25f;
	private int lightCharges = 0;

	private Image powerUI;

	public ParticleSystem pulse;
	public ParticleSystem aura;
	public ParticleSystem dust;

	OVRHapticsClip clip;

	public AudioClip hapticsSFX;

	Timer blastCD;
	GameObject blastReadyVFX;
	AudioClip triggerSFX;
	AudioClip blastSFX;
	Text CD;

	// Use this for initialization
	void Start () {
		blastReadyVFX = GameObject.Find ("Light Ready");
		blastReadyVFX.SetActive (false);
		blastCD = new Timer (30.0f);
		CD = GameObject.Find ("Blast CD").GetComponent<Text> ();
		triggerSFX = (AudioClip)Resources.Load ("Audio/sfx-lightswitch");
		blastSFX = (AudioClip)Resources.Load ("Audio/Light-BlastFX");
		light = transform.GetChild(0).GetComponent<FlashlightCollider>();
		powerUI = GameObject.Find ("Power Remaining").GetComponent<Image> ();
		powerUI.fillAmount = flashlightPower / 100.0f;
	}
	
	// Update is called once per frame
	void Update () {

		// Core flashlight functionality
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

		// Light blast functionality
		if (lightCharges > 0) {
			powerUI.color = new Color (1, 0.92f, 0.016f, 1);
			blastReadyVFX.SetActive (true);
		}
		if (lightCharges <= 0) {
			powerUI.color = new Color (1, 1, 1, 1);
			blastReadyVFX.SetActive (false);
		}
		if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) && (lightCharges > 0) && (!blastCD.IsRunning())) {
			lightCharges--;
			aura.Play ();
			pulse.Play ();
			dust.Play ();
			GetComponent<AudioSource> ().PlayOneShot (blastSFX, 0.2f);
			vibrate ();
			LightExplosion();
			blastCD.StartTimer ();
		}
		if (blastCD.IsRunning ()) {
			blastCD.UpdateTimer ();
			CD.text = ((int)blastCD.GetTimeRemaining ()).ToString();
		}
	}

	public void AddPower (float power) {
		flashlightPower = Mathf.Clamp(flashlightPower + power, 0.0f, 100.0f);
		powerUI.fillAmount = flashlightPower / 100.0f;
	}

	public float GetPower () {
		return flashlightPower;
	}

	public void AddLightCharge () {
		lightCharges++;
		lightCharges = Mathf.Clamp (lightCharges, 0, 1);
	}

	public void LightExplosion () {
		
		Collider[] colliders = Physics.OverlapSphere (transform.position, 10.0f);
		foreach (Collider hit in colliders) {
			if (hit.gameObject.tag == "Unit") {
				Rigidbody rb = hit.GetComponent<Rigidbody> ();
				rb.GetComponent<Ghost> ().Damage (5.0f);
				if (rb != null) {
					StartCoroutine(CapsuleDelay(rb));
				}
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
			rb.AddExplosionForce (40.0f, Vector3.zero, 10.0f, 0.0f, ForceMode.Impulse);
		}
		yield return new WaitForSeconds (3.0f);
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
		OVRHaptics.RightChannel.Clear ();
	}

	public void vibrate () {
		clip = new OVRHapticsClip (hapticsSFX);
		OVRHaptics.RightChannel.Mix (clip);
	}

}
