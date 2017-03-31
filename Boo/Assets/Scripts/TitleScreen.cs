using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {

	bool isPlayingIntro;

	void Start () {
		isPlayingIntro = true;
	}

	// Update is called once per frame
	void Update () {
		if (isPlayingIntro) {
			Color curColor = GetComponent<Image> ().color;
			float alphaDiff = Mathf.Abs (curColor.a - 1.0f);
			if (alphaDiff > 0.0001f) {
				curColor.a = Mathf.Lerp (curColor.a, 0.8f, 0.1f * Time.deltaTime);
				GetComponent<Image> ().color = curColor;
			} else {
				isPlayingIntro = false;
			}
		} else {

		}
	}
}
