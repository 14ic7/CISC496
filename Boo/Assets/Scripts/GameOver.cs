using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameOver : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		Color curColor = this.GetComponent<Image>().color;
		float alphaDiff = Mathf.Abs (curColor.a - 1.0f);
			if (alphaDiff > 0.0001f) {
				curColor.a = Mathf.Lerp (curColor.a, 1.0f, 0.5f * Time.deltaTime);
				this.GetComponent<Image>().color = curColor;
			}
	}

}
