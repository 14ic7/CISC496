using UnityEngine;
using System.Collections;

public class NoDestroy : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}

}
