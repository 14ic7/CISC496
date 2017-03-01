using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections.Generic;
using System;

public class Ghost : MonoBehaviour {
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
	
	//set coloured outline on shader
	public void setHighlight(float width) {
		material.SetFloat("_Outline", width);
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.tag == "Player") {
			collision.gameObject.GetComponent<PlayerHealth> ().Damage (20);	// Deal 20 points of damage on collision.
		}
	}
}
