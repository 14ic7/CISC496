using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RTSTitle : MonoBehaviour {

	Button playButton;
	Button quitButton;
	Button tutorialButton;

	bool RTSReady;

	// Use this for initialization
	void Start () {
		/* playButton = GameObject.Find ("Play RTS").GetComponent<Button> ();
		quitButton = GameObject.Find ("Quit RTS").GetComponent<Button> ();
		tutorialButton = GameObject.Find ("Tutorial RTS").GetComponent<Button> ();

		playButton.onClick.AddListener (delegate {
			playGame ();
		});
		quitButton.onClick.AddListener (delegate {
			quitGame ();
		});
		tutorialButton.onClick.AddListener (delegate {
			tutorialGame ();
		}); */
	}

	public void playGame () {
		VRTitle VRHandler = GameObject.Find ("OVRCameraRig").GetComponent<VRTitle> ();
		VRHandler.setRTSReady ();
		Debug.Log ("waiting for VR");
		// set waiting for VR player msg
	}

	public void quitGame () {
		Application.Quit ();
	}

	public void tutorialGame () {
		SceneManager.LoadScene ("VR Tutorial");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
