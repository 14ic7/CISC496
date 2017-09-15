using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Oculus.Platform;

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
	GameObject explanationText;

	bool VRReady;
	bool RTSReady;

	// Use this for initialization
	void Start ()
	{
		// DRM for the Oculus store (sends validation key to Oculus server)
		Oculus.Platform.Core.AsyncInitialize("1416957925026797");
        Oculus.Platform.Entitlements.IsUserEntitledToApplication().OnComplete(OculusDRMCallback);


		VRReady = false;
		RTSReady = false;
		selected = 0;
		play = GameObject.Find ("Play_Tombstone (Selected)");
		exit = GameObject.Find ("Quit_Tombstone (Selected)");
		tutorial = GameObject.Find ("avatar-static_v2 (Selected)");
		reaperModel = GameObject.Find ("avatar-static_v2");
		playText = GameObject.Find ("Start Game");
		exitText = GameObject.Find ("Quit Game");
		tutorialText = GameObject.Find ("Tutorial");
		explanationText = GameObject.Find ("Explanation");
	}
	
	// Update is called once per frame
	void Update () {
		if (!VRReady) {
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
			if (OVRInput.GetDown (OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && (selected == 0)) {
				// DestroyImmediate (GameObject.Find ("BGM"));
				VRReady = true;
				explanationText.GetComponent<Text>().text = "Waiting for the RTS player to ready up...";
				checkBothReady();
				// SceneManager.LoadScene ("VR");
			} else if (OVRInput.GetDown (OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && (selected == 1)) {
				UnityEngine.Application.Quit();
			} else if (OVRInput.GetDown (OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && (selected == 2))
			{
				enabled = false;
				SceneManager.LoadScene("VR Tutorial", LoadSceneMode.Additive);
			}
		}
	}

	public void setRTSReady () {
		RTSReady = true;
		checkBothReady();
	}

	void checkBothReady()
	{
		if (VRReady && RTSReady)
		{
			GameObject BGM = GameObject.Find("BGM");
			BGM.GetComponent<AudioSource>().Stop();
			Destroy(BGM);

			SceneManager.LoadScene("VR");
		}
	}

	void OculusDRMCallback(Message msg)
	{
		if (msg.IsError)
		{
			// Entitlement check failed
		}
		else
		{
			// Entitlement check passed
		}
	}
}
