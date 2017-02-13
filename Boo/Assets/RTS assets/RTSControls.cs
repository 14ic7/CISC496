using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

public class RTSControls : MonoBehaviour {
	const float HIGHLIGHT_WIDTH = 0.02f;

	//TODO: scroll camera around using mouse
	public LayerMask terrainMask;
	AICharacterControl hoverHighlight; //unit mouse is hovering over
	HashSet<AICharacterControl> selectedUnits = new HashSet<AICharacterControl>();
	
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitData;
		AICharacterControl unit = null;

		if (Physics.Raycast(ray, out hitData)) {
			unit = hitData.transform.GetComponent<AICharacterControl>();
		}

		//if unit under mouse has changed
		if (hoverHighlight != null && (unit == null || hoverHighlight != unit)) {
			//unhighlight it if not selected
			if (!selectedUnits.Contains(hoverHighlight)) {
				setHighlight(hoverHighlight, 0);
			}
			hoverHighlight = null;
		}

		//if mouse is hovering over unit and nothing is highlighted
		if (unit != null && hoverHighlight == null) {
			//highlight this unit
			hoverHighlight = unit;
			setHighlight(unit, HIGHLIGHT_WIDTH);
		}

		//if left mouse click while hovering over unit
		if (Input.GetMouseButtonDown(0) && unit != null) {
			//add unit to selected list
			Debug.Log("unit hit!");
			selectedUnits.Add(unit); //HashSet, no redundancies
			setHighlight(unit, HIGHLIGHT_WIDTH);
			hoverHighlight = null;
		}

		//if left mouse click off unit
		if (Input.GetMouseButtonDown(0) && unit == null) {
			Debug.Log("Deselect All");
			//remove highlight
			foreach (AICharacterControl unit0 in selectedUnits) {
				setHighlight(unit0, 0);
			}

			selectedUnits.Clear(); //deslect all
		}

		//right mouse (set waypoint)
		if (Input.GetMouseButtonDown(1)) {
			if (Physics.Raycast(ray, out hitData, 10000f, terrainMask)) {
				Debug.Log("terrain hit");
				foreach (AICharacterControl unit0 in selectedUnits) {
					unit0.SetTarget(hitData.point);
				}
			}
		}
	}

	void setHighlight(AICharacterControl unit, float width) {
		if (unit != null) {
			unit.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_Outline", width);
		}
	}
}
