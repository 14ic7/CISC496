using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections.Generic;
using System;

public class Ghost : MonoBehaviour {
	const float FULL_HEALTH = 10;
	readonly Color LIGHT_BLUE = new Color(0, 0.6f, 1, 0.32f);
	readonly Color BLOOD_RED = new Color(1, 0, 0, 0.49f);

	float health = FULL_HEALTH;

	Material material;
	AICharacterControl AIScript;

	void Start () {
		material = transform.GetChild(0).GetComponent<MeshRenderer>().material;
		AIScript = GetComponent<AICharacterControl>();
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
}
