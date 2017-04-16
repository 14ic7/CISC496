using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VacuumController : MonoBehaviour {

	VacuumCollider vacuum;
	LightCollector lc;

	OVRHapticsClip clip;

	int bombs = 0;
	GameObject bombNozzle;

	public AudioClip hapticsSFX;

	// Use this for initialization
	void Start () {
		vacuum = transform.GetChild (0).GetComponent<VacuumCollider> ();
		lc = transform.GetChild (1).GetComponent<LightCollector> ();
		vacuum.turnOff ();
		lc.turnOff ();
		bombNozzle = GameObject.Find ("Bomb Nozzle");

		// set ghosts killed UI number
		GameObject.Find("Ghosts Killed").GetComponent<Text>().text = GameObject.FindGameObjectsWithTag(RTSControls.UNIT_TAG).Length.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) {
			turnOn();
			vibrate();
			GetComponent<AudioSource> ().Play();
		}  else if (Input.GetKey(KeyCode.F)) {
			turnOn();
 		} else {
			vacuum.turnOff();
			lc.turnOff();
			OVRHaptics.LeftChannel.Clear();
			GetComponent<AudioSource> ().Stop ();
		}

		if (OVRInput.GetDown (OVRInput.Button.PrimaryHandTrigger) && (bombs > 0)) {
			bombs--;
			GameObject.Find ("Bombs Left").GetComponent<Text> ().text = bombs.ToString ();
			GameObject primedBomb = Instantiate (Resources.Load ("Primed Bomb"), bombNozzle.transform.position, bombNozzle.transform.rotation) as GameObject;
			primedBomb.GetComponent<Rigidbody> ().AddForce (transform.forward * 50.0f, ForceMode.Impulse);
			// play shooting sfx
		}
	}

	public void AddBomb () {
		bombs++;
		GameObject.Find ("Bombs Left").GetComponent<Text> ().text = bombs.ToString ();
	}

	void turnOn() {
		vacuum.turnOn();
		lc.turnOn();
	}

	public void vibrate () {
		clip = new OVRHapticsClip (hapticsSFX);
		OVRHaptics.LeftChannel.Mix (clip);
	}
		
}
