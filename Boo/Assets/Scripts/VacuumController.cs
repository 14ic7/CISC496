using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VacuumController : MonoBehaviour {

	VacuumCollider vacuum;
	LightCollector lc;

	GameObject vfx;

	OVRHapticsClip clip;
	public AudioClip hapticsSFX;

	// Use this for initialization
	void Start () {
		vacuum = transform.GetChild (0).GetComponent<VacuumCollider> ();
		lc = transform.GetChild (1).GetComponent<LightCollector> ();
		vfx = GameObject.Find ("Vacuum FX");
		vfx.SetActive (false);
		vacuum.turnOff ();
		lc.turnOff ();

		// set ghosts killed UI number
		GameObject.Find("Ghosts Killed").GetComponent<Text>().text = GameObject.FindGameObjectsWithTag(RTSControls.UNIT_TAG).Length.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) {
			turnOn();
			vibrate();
		}  else if (Input.GetKey(KeyCode.F)) {
			turnOn();
 		} else {
			vacuum.turnOff();
			lc.turnOff();
			vfx.SetActive(false);
			OVRHaptics.LeftChannel.Clear();
		}
	}

	void turnOn() {
		vacuum.turnOn();
		lc.turnOn();
		vfx.SetActive(true);
	}

	public void vibrate () {
		clip = new OVRHapticsClip (hapticsSFX);
		OVRHaptics.LeftChannel.Mix (clip);
	}
		
}
