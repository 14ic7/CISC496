using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSControls : MonoBehaviour {
	const float HIGHLIGHT_WIDTH = 0.02f;
	const string UNIT_TAG = "Unit";
	Texture2D SELECT_TEXTURE;

	//TODO: scroll camera around using mouse
	public LayerMask TERRAIN_MASK;
	public Camera RTSCamera;

	Vector2 mouseDragOrigin; //start pos of a mouse drag
	Ghost hoverHighlight; //unit mouse is hovering over
	HashSet<Ghost> highlightedUnits = new HashSet<Ghost>(); //units inside selection box
	HashSet<Ghost> selectedUnits = new HashSet<Ghost>();




	// --------------- INITIALIZATION ---------------

	void Awake() {
		//create texture for mouse drag box
		SELECT_TEXTURE = new Texture2D(1, 1); //1x1 pixels
		SELECT_TEXTURE.SetPixel(0, 0, new Color(0, 0.6f, 1, 0.32f)); //light blue
		SELECT_TEXTURE.Apply();
	}



	// --------------- FRAME LOOP ---------------

	//Draw mouse drag rect
	void OnGUI() {
		if (Input.GetMouseButton(0) && Vector2.Distance(mouseDragOrigin, Input.mousePosition) > 6) {
			//rect in GUI coordinates
			GUI.DrawTexture(
				new Rect(
					mouseDragOrigin.x, //x
					Screen.height - mouseDragOrigin.y, //y
					Input.mousePosition.x - mouseDragOrigin.x, //width
					mouseDragOrigin.y - Input.mousePosition.y //height
				),
				SELECT_TEXTURE //light blue
			);
		}
	}

	/**
	 * highlight = mouse is hovering over unit / unit is inside selection box
	 * selection = unit is selected and under player control
	 **/
	void Update() {
		Ray ray = RTSCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitData;
		Ghost unit = null;

		//perform raycast
		if (Physics.Raycast(ray, out hitData)) {
			unit = hitData.transform.GetComponent<Ghost>();
		}

		//handle highlighting
		if (!Input.GetMouseButton(0)) {
			//if unit under mouse has changed
			if (hoverHighlight != unit) {
				//unhighlight it if not selected
				setHoverHighlightNull();
			}

			//if mouse is hovering over unit and nothing is highlighted
			if (unit != null && hoverHighlight == null) {
				//highlight this unit
				hoverHighlight = unit;
				unit.setHighlight(HIGHLIGHT_WIDTH);
			}
		}

		//left mouse click
		if (Input.GetMouseButtonDown(0)) {
			//save mouse position
			mouseDragOrigin = Input.mousePosition;

			deselectAll();

			//if hovering over unit
			if (unit != null) {
				//select only this unit
				Debug.Log("unit hit!");
				selectedUnits.Add(unit);
				unit.setHighlight(HIGHLIGHT_WIDTH);
				hoverHighlight = null;
			}
		}

		//mouse held or released (drag) at distance > 6
		if ((Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) && Vector2.Distance(mouseDragOrigin, Input.mousePosition) > 6) {
			setHoverHighlightNull();
			unhighlightAll();
			deselectAll();

			//rect in screen coordinates
			Rect rect = new Rect(mouseDragOrigin, (Vector2)Input.mousePosition - mouseDragOrigin);

			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag(UNIT_TAG)) { //FindWithTag is fast since unity caches things by tag
				if (rect.Contains(RTSCamera.WorldToScreenPoint(gameObject.transform.position), true)) {
					Ghost unit0 = gameObject.GetComponent<Ghost>();
					unit0.setHighlight(HIGHLIGHT_WIDTH);
					highlightedUnits.Add(unit0);
				}
			}

			//mouse release (after drag)
			if (Input.GetMouseButtonUp(0)) {
				selectedUnits = highlightedUnits;
			}
		}

		//right mouse (set waypoint)
		if (Input.GetMouseButtonDown(1)) {
			if (Physics.Raycast(ray, out hitData, 10000f, TERRAIN_MASK)) {
				Debug.Log("terrain hit");
				foreach (Ghost unit0 in selectedUnits) {
					unit0.SetTarget(hitData.point);
				}
			}
		}
	}



	// -------------- HELPER FUNCTIONS ---------------

	void setHoverHighlightNull() {
		//if unit is highlighted and not selected
		if (hoverHighlight != null && !selectedUnits.Contains(hoverHighlight)) {
			hoverHighlight.setHighlight(0);
			hoverHighlight = null;
		}
	}

	void unhighlightAll() {
		//remove highlight
		foreach (Ghost unit in highlightedUnits) {
			unit.setHighlight(0);
		}

		//deselect all
		highlightedUnits.Clear();
	}


	void deselectAll() {
		//remove highlight
		foreach (Ghost unit in selectedUnits) {
			unit.setHighlight(0);
		}

		//deselect all
		selectedUnits.Clear();
	}
}
