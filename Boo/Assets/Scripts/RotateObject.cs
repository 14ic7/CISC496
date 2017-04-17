using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0, 0, 70 * Time.deltaTime));
	}
}
