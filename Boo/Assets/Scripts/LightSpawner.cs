using UnityEngine;
using System.Collections;

public class LightSpawner : MonoBehaviour {

	private Timer spawnDelay;
	Vector3 center;

	public GameObject lightPrefab;


	// Use this for initialization
	void Start () {
		spawnDelay = new Timer (10.0f);
		center = transform.position;
		spawnDelay.StartTimer ();
	}
	
	// Update is called once per frame
	void Update () {
		if (spawnDelay.IsRunning ()) {
			spawnDelay.UpdateTimer ();
		} else {
			Vector3 pos = RandomCircle (center, 10.0f);
			Quaternion rot = Quaternion.FromToRotation (Vector3.forward, center - pos);
			Instantiate (lightPrefab, pos, rot);
			spawnDelay.ResetTimer ();
		}
	}

	Vector3 RandomCircle (Vector3 c, float r) {
		float ang = Random.value * 360;
		Vector3 pos;
		pos.x = c.x + r * Mathf.Sin (ang * Mathf.Deg2Rad);
		pos.y = c.y + r * Mathf.Sin (ang * Mathf.Deg2Rad);
		pos.z = c.z;
		return pos;
	}
}
