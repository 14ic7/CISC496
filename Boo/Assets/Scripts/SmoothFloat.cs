using UnityEngine;
using System.Collections;

public class SmoothFloat : MonoBehaviour {

	public float amplitude;
	public float speed;

	private float yStart;

	// Use this for initialization
	void Start () {
		yStart = this.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3 (this.transform.position.x, yStart + amplitude * Mathf.Sin (speed * Time.time), this.transform.position.z);

	}
}
