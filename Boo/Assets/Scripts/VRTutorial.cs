using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VRTutorial : MonoBehaviour {

	GameObject centerEyeAnchor;

	// Use this for initialization
	void Start () {
		centerEyeAnchor = GameObject.Find ("CenterEyeAnchor");
	}
	
	// Update is called once per frame
	void Update () {
		if ((centerEyeAnchor.transform.eulerAngles.y > 160.0f && centerEyeAnchor.transform.eulerAngles.y < 200.0f) && OVRInput.Get (OVRInput.Button.SecondaryIndexTrigger))
		{
			SceneManager.UnloadSceneAsync("VR Tutorial");
		}
	}
}
