using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class VRTutorial : MonoBehaviour {

	GameObject centerEyeAnchor;

	// Use this for initialization
	void Start () {
		centerEyeAnchor = GameObject.Find ("CenterEyeAnchor");
	}
	
	// Update is called once per frame
	void Update () {
		if ((centerEyeAnchor.transform.eulerAngles.y > 230f && centerEyeAnchor.transform.eulerAngles.y < 290f) && OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger))
		{
			FindObjectOfType<VRTitle>().enabled = true;
			Debug.Log("Unload me there brother");
			SceneManager.UnloadSceneAsync("VR Tutorial");
		}
	}
}
