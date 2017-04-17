using UnityEngine;
using System.Collections;

public class DespawnLight : MonoBehaviour {

	Timer despawnTimer;
	ParticleSystem ps;

	// Use this for initialization
	void Start () {
		despawnTimer = new Timer (15.0f);
		ps = this.transform.GetChild (0).GetComponent<ParticleSystem> ();
		despawnTimer.StartTimer ();
	}
	
	// Update is called once per frame
	void Update () {
		if (despawnTimer.IsRunning ()) {
			despawnTimer.UpdateTimer ();
		} else {
			ps.Stop (true);
			Destroy (this.gameObject, 1.0f);
		}
	}
}
