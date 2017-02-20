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
}
