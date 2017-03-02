using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSControls : MonoBehaviour {
	const float PAN_SPEED_RATIO = 0.0215f; //camera pans faster as it moves farther from the scene
	const float HIGHLIGHT_WIDTH = 0.02f;
	const string UNIT_TAG = "Unit";
	Texture2D SELECT_TEXTURE;
	
	public LayerMask TERRAIN_MASK;
	public Camera RTSCamera;

	float cameraPanSpeed; //camera speed (parallel to the ground)
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

		// apparent speed must be maintained! http://physics.stackexchange.com/a/188075
		// actual speed = distance * apparent speed
		cameraPanSpeed = RTSCamera.transform.position.y * PAN_SPEED_RATIO;
	}



	// --------------- FRAME LOOP ---------------

	 
	/// Draw mouse drag rect, scroll camera in/out.
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

		//scroll wheel zooms camera
		Event ev = Event.current;
		if (ev.type == EventType.ScrollWheel) {
			Vector3 newPos = RTSCamera.transform.position - RTSCamera.transform.forward * ev.delta.y * 0.25f;

			//limit y position to between 6 and 63
			if (newPos.y > 6 && newPos.y < 63) {
				RTSCamera.transform.position = newPos;

				// apparent speed must be maintained! http://physics.stackexchange.com/a/188075
				// actual speed = distance * apparent speed
				cameraPanSpeed = newPos.y * PAN_SPEED_RATIO;
			}
		}
	}

	/// highlight = mouse is hovering over unit / unit is inside selection box
	/// selection = unit is selected and under player control
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
				selectedUnits.Add(unit);
				unit.hurt(1);
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
				foreach (Ghost unit0 in selectedUnits) {
					unit0.SetTarget(hitData.point);
				}
			}
		}



		//pan camera when mouse is at screen edge

		Vector3 panVector = Vector2.zero;

		if (Input.mousePosition.x <= 0) {
			panVector += -Vector3.right;
		} else if (Input.mousePosition.x >= Screen.width) {
			panVector += Vector3.right;
		}

		if (Input.mousePosition.y <= 0) {
			panVector += -Vector3.forward;
		} else if (Input.mousePosition.y >= Screen.height) {
			panVector += Vector3.forward;
		}

		RTSCamera.transform.position += panVector.normalized * cameraPanSpeed;
	}



	// -------------- HELPER FUNCTIONS ---------------

	void setHoverHighlightNull() {
		//if unit is highlighted and not selected
		if (hoverHighlight != null && !selectedUnits.Contains(hoverHighlight)) {
			hoverHighlight.setHighlight(0);
		}
		hoverHighlight = null;
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
