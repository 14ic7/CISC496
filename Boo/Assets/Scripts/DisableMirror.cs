using UnityEngine;
using System.Collections;

public class DisableMirror : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForEndOfFrame ();
		UnityEngine.VR.VRSettings.showDeviceView = false;
	}

}
