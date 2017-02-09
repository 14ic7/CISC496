using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

public class RTSControls : MonoBehaviour {
	public LayerMask terrainMask;

	HashSet<AICharacterControl> selectedUnits = new HashSet<AICharacterControl>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//left mouse (select)
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				AICharacterControl unit = hit.transform.GetComponent<AICharacterControl>();
				if (unit) {
					Debug.Log("unit hit!");
					selectedUnits.Add(unit); //HashSet, no redundancies
					unit.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.57f);
				} else {
					deselectAll();
				}
			} else {
				deselectAll();
			}
		}



		//right mouse (set waypoint)
		if (Input.GetMouseButtonDown(1)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 10000f, terrainMask)) { // farClipPlane = geometry too far to see
				Debug.Log("terrain hit");
				foreach (AICharacterControl unit in selectedUnits) {
					unit.SetTarget(hit.point);
				}
			}
		}
	}

	void deselectAll() {
		Debug.Log("Deselect All");
		//remove highlight
		foreach (AICharacterControl unit in selectedUnits) {
			unit.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0f);
		}
		selectedUnits.Clear(); //deslect all
	}
}
