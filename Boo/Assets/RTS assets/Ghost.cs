using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections.Generic;
using System;

public class Ghost : MonoBehaviour {
	const float FULL_HEALTH = 10;
	const float BOB_SPEED = 0.02f;
	const float BOB_RANGE = 0.1f;
	readonly Color LIGHT_BLUE = new Color(0, 0.6f, 1, 0.32f);
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
		material = child.GetChild(1).GetComponent<SkinnedMeshRenderer>().material;

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

	public void hurt(float damage) {
		health -= damage;
		if (health <= 0) {
			//TODO
			Debug.Log("kill");
		}
		
		material.SetVector("_OutlineColor", Color.Lerp(BLOOD_RED, LIGHT_BLUE, health/FULL_HEALTH));
	}
	
	//set coloured outline on shader
	public void setHighlight(float width) {
		material.SetFloat("_Outline", width);
	}
	public void setHighlight(Color colour) {
		material.SetVector("_OutlineColor", colour);
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.tag == "Player") {
			collision.gameObject.GetComponent<PlayerHealth> ().Damage (20);	// Deal 20 points of damage on collision.
		}
	}
}
