using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameOver : MonoBehaviour {

	Timer delayInput;

	void Start () {
		delayInput = new Timer (5.0f);
		delayInput.StartTimer ();
	}
	
	// Update is called once per frame
	void Update () {
		delayInput.UpdateTimer ();
		Color curColor = GetComponent<Image> ().color;
		float alphaDiff = Mathf.Abs (curColor.a - 1.0f);
		if (alphaDiff > 0.0001f) {
			curColor.a = Mathf.Lerp (curColor.a, 0.8f, 0.5f * Time.deltaTime);
			GetComponent<Image> ().color = curColor;
		}
		if (OVRInput.GetDown (OVRInput.Button.PrimaryIndexTrigger) && !(delayInput.IsRunning())) {
			Application.Quit();
		}
		if (OVRInput.GetDown (OVRInput.Button.SecondaryIndexTrigger) && !(delayInput.IsRunning())) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);	
		}
	}
		
}
