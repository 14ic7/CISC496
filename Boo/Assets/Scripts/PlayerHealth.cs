using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	private float maxHP = 100.0f;
	private float currHP = 100.0f;
	private float regenRateSlow = 1.5f;		// Regen rate after having been recently damaged.
	private float regenRateFast = 10.0f;	// Regen rate after not having taken damage for a certain amount of time.
	private float damageDelay = 5.0f;		// Time after taking damage before fast regen begins.
	private Timer timeSinceLastDamage;

	// Timer must be initialized before anything else, or else null reference exception.
	void Awake () {
		timeSinceLastDamage = new Timer (damageDelay);
	}
	
	// Update is called once per frame
	void Update () {
		if (currHP <= 1.0f) {
			// Game over!
			Debug.Log("game over!");
		}
		if (timeSinceLastDamage.IsRunning () == true) {
			timeSinceLastDamage.UpdateTimer ();
			currHP = Mathf.Clamp ((currHP + (regenRateSlow * Time.deltaTime)), 0.0f, maxHP);
		} else {
			currHP = Mathf.Clamp ((currHP + (regenRateFast * Time.deltaTime)), 0.0f, maxHP);
		}
	}

	// Inflict damage on the player character. Can be called outside this script.
	public void Damage (float damage) {
		currHP -= damage;
		// If the timer is still running, reset the timer since they just took damage within the threshold.
		if (timeSinceLastDamage.IsRunning () == true) {
			timeSinceLastDamage.ResetTimer ();
		} else {	// Else, start the timer now.
			timeSinceLastDamage.StartTimer ();
		}
	}
}
