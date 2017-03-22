using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections.Generic;
using System;

public class Ghost : MonoBehaviour {
	public static readonly Color LIGHT_BLUE = new Color(0, 0.6f, 1, 0.32f);

	const string RTS_UI_NAME = "RTSUI";
	const float FULL_HEALTH = 10;
	const float BOB_SPEED = 0.02f;
	const float BOB_RANGE = 0.1f;
	readonly Color PURPLE = new Color32(144, 66, 244, 82); // purple = stunned
	readonly Color BLOOD_RED = new Color(1, 0, 0, 0.49f);

	float bobHeight;
	float health = FULL_HEALTH;
	float bobProgress;

	Material material;
	AICharacterControl AIScript;
	Transform child;

	void Start () {
		bobProgress = UnityEngine.Random.Range(0, Mathf.PI*2);

		child = transform.GetChild(0);
		bobHeight = child.transform.position.y;
		material = child.GetComponent<MeshRenderer>().materials[1];

		AIScript = GetComponent<AICharacterControl>();
		SetTarget(transform.position);
	}

	void Update() {
		bobProgress += BOB_SPEED;

		Vector3 newPos = child.position;
		newPos.y = bobHeight + Mathf.Sin(bobProgress)*BOB_RANGE;
		child.position = newPos;
	}

	//set navmesh target position
	public void SetTarget(Vector3 target) {
		AIScript.SetTarget(target);
	}

	public void stun() {
		Debug.Log("stun "+name);
		GetComponent<NavMeshAgent>().Stop();
		setHighlight(PURPLE);
	}

	public void unstun() {
		Debug.Log("unstun "+name);
		GetComponent<NavMeshAgent>().Resume();
		setHighlight(false);
	}

	public void hurt(float damage) {
		health -= damage;
		if (health <= 0) {
			Destroy(gameObject);

			//if all units dead, display "You Lose" message
			//check length <= 1 since this object isn't destroyed until the end of the frame
			if (GameObject.FindGameObjectsWithTag(RTSControls.UNIT_TAG).Length <= 1) {
				GameObject.Find(RTS_UI_NAME).GetComponent<Canvas>().enabled = true;
			}
		} else {
			setHighlight(true);
		}
	}
	
	//set coloured outline on shader
	public void setHighlight(Color colour) {
		material.color = colour;
	}
	public void setHighlight(bool value) {
		if (value) {
			material.color = Color.Lerp(BLOOD_RED, LIGHT_BLUE, health/FULL_HEALTH);
		} else {
			material.color = Color.white;
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.tag == "Player") {
			collision.gameObject.GetComponent<PlayerHealth> ().Damage (20);	// Deal 20 points of damage on collision.
		}
	}
}
