using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VRTitle : MonoBehaviour {

	int playTombstone = 0;
	int exitTombstone = 1;
	int tutorialTombstone = 2;
	int selected;

	GameObject play;
	GameObject exit;
	GameObject tutorial;
	GameObject reaperModel;
	GameObject playText;
	GameObject exitText;
	GameObject tutorialText;

	// Use this for initialization
	void Start () {
		selected = 0;
		play = GameObject.Find ("Play_Tombstone (Selected)");
		exit = GameObject.Find ("Quit_Tombstone (Selected)");
		tutorial = GameObject.Find ("avatar-static_v2 (Selected)");
		reaperModel = GameObject.Find ("avatar-static_v2");
		playText = GameObject.Find ("Start Game");
		exitText = GameObject.Find ("Quit Game");
		tutorialText = GameObject.Find ("Tutorial");
	}
	
	// Update is called once per frame
	void Update () {
		if (OVRInput.GetDown (OVRInput.Button.SecondaryThumbstickRight)) {
			selected += 1;
			if (selected == 3) {
				selected = 0;
			}
		} else if (OVRInput.GetDown (OVRInput.Button.SecondaryThumbstickLeft)) {
			selected -= 1;
			if (selected == -1) {
				selected = 2;
			}
		}
		if (selected == 0) {
			play.SetActive (true);
			exit.SetActive (false);
			tutorial.SetActive (false);
			reaperModel.SetActive (true);
			playText.SetActive (true);
			exitText.SetActive (false);
			tutorialText.SetActive (false);
		} else if (selected == 1) {
			exit.SetActive (true);
			play.SetActive (false);
			tutorial.SetActive (false);
			reaperModel.SetActive (true);
			exitText.SetActive (true);
			playText.SetActive (false);
			tutorialText.SetActive (false);
		} else if (selected == 2) {
			tutorial.SetActive (true);
			play.SetActive (false);
			exit.SetActive (false);
			reaperModel.SetActive (false);
			tutorialText.SetActive (true);
			playText.SetActive (false);
			exitText.SetActive (false);
		}
		if (OVRInput.GetDown (OVRInput.Button.One) && (selected == 0)) {
			Destroy (GameObject.Find ("BGM"));
			SceneManager.LoadScene ("VR");
		} else if (OVRInput.GetDown (OVRInput.Button.One) && (selected == 1)) {
			Application.Quit ();
		} else if (OVRInput.GetDown (OVRInput.Button.One) && (selected == 2)) {
			SceneManager.LoadScene ("VR Tutorial");
		}
	}
}
