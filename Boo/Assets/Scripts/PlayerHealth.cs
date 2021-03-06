﻿using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : RTSEnemy {
	private float maxHP = 100.0f;
	private float currHP = 100.0f;
	private float regenRateSlow = 1.5f;		// Regen rate after having been recently damaged.
	private float regenRateFast = 10.0f;	// Regen rate after not having taken damage for a certain amount of time.
	private float damageDelay = 5.0f;		// Time after taking damage before fast regen begins.
	private Timer timeSinceLastDamage;

	private Transform vrCamera;
	private Image damageUI;
	private Color curColor;
	private Material coatMaterial;
	private Color coatColour;
	private HealthBar healthBar;

	public Sprite VRloss;

	// Timer must be initialized before anything else, or else null reference exception.
	void Awake () {
		healthBar = GetComponentInChildren<HealthBar>();

		timeSinceLastDamage = new Timer (damageDelay);
		damageUI = GameObject.Find ("Damage Flash").GetComponent<Image> ();

		vrCamera = GameObject.Find("CenterEyeAnchor").transform;
		curColor = damageUI.color;

		coatMaterial = transform.GetComponent<MeshRenderer>().materials.Single(material => material.name == "black (Instance)");
		coatColour = coatMaterial.color;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// snap character model to headset position
		Vector3 position = vrCamera.position;
		position.y = transform.position.y;
		transform.position = position;
		
		if (currHP <= 1.0f) {
			GameObject.Find ("Game Over (VR)").GetComponent<Image>().sprite = VRloss;
			GameObject.Find ("Game Over (VR)").GetComponent<GameOver> ().enabled = true;

			GameObject.Find("WinLoseScreen").GetComponent<RTSWinLose>().win();
		}
		if (timeSinceLastDamage.IsRunning () == true) {
			timeSinceLastDamage.UpdateTimer ();
			currHP = Mathf.Clamp ((currHP + (regenRateSlow * Time.deltaTime)), 0.0f, maxHP);
		} else {
			currHP = Mathf.Clamp ((currHP + (regenRateFast * Time.deltaTime)), 0.0f, maxHP);
		}
		curColor.a = Mathf.Lerp(curColor.a, 1 - (currHP / 100), 6.0f * Time.deltaTime);	// Make the screen progressively more red as you get more damaged.
		damageUI.color = curColor;
		if (currHP < 25.0f) {	// If the players' HP is under 25%, the screen will flash, notifying critical health.
			curColor.a = Mathf.Lerp (curColor.a, UnityEngine.Random.Range (-0.4f, 0.4f) + (1 - (currHP / 100)), 1.5f * Time.deltaTime);
			damageUI.color = curColor;
		}
	}

	// Inflict damage on the player character. Can be called outside this script.
	public override void Damage (float damage) {
		currHP -= damage;

		healthBar.health = currHP / maxHP;

		// If the timer is still running, reset the timer since they just took damage within the threshold.
		if (timeSinceLastDamage.IsRunning () == true) {
			timeSinceLastDamage.ResetTimer ();
		} else {	// Else, start the timer now.
			timeSinceLastDamage.StartTimer ();
		}
	}

	public override void setHighlight(Color colour) {
		coatMaterial.color = colour;
	}

	public override void setHighlight(bool value) {
		if (value) {
			coatMaterial.color = Color.red;
		} else {
			coatMaterial.color = coatColour;
		}
	}
}
