using UnityEngine;
using System.Collections;

public class GhostDeath : MonoBehaviour {
	public string TARGET_NAME;

	Vector3 startPos;
	Vector3 startScale;
	Transform target;
	float time;

	void OnEnable () {
		startPos = transform.position;
		startScale = transform.localScale;
		target = GameObject.Find(TARGET_NAME).transform;
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (time > 1) {
			Destroy(gameObject);
		} else {
			time += Time.deltaTime;
			transform.position = Vector3.Lerp(startPos, target.position, time);
			transform.localScale = Vector3.Lerp(startScale, Vector3.zero, time);
		}
	}
}
