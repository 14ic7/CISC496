using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RTSControls : MonoBehaviour {
	public static readonly string UNIT_TAG = "Unit";

	readonly Rect WORLD_BOUNDS = new Rect(-72, -72, 144, 144);
	const float PAN_SPEED_RATIO = 2.5f; //camera pans faster as it moves farther from the scene
	const float HIGHLIGHT_WIDTH = 0.02f;
	Texture2D SELECT_TEXTURE;
	LayerMask TERRAIN_MASK;
	LayerMask SELECTABLE_MASK;

	float cameraPanSpeed; //camera speed (parallel to the ground)
	Vector2 mouseDragOrigin; //start pos of a mouse drag
	RTSEntity hoverHighlight; //unit mouse is hovering over
	HashSet<Ghost> highlightedUnits = new HashSet<Ghost>(); //units inside selection box
	HashSet<Ghost> selectedUnits = new HashSet<Ghost>();

	Camera RTSCamera;
	Transform yellowCircle;



	// --------------- INITIALIZATION ---------------

	void Awake() {
		// create texture for mouse drag box
		SELECT_TEXTURE = new Texture2D(1, 1); //1x1 pixels
		SELECT_TEXTURE.SetPixel(0, 0, Ghost.LIGHT_BLUE); //light blue
		SELECT_TEXTURE.Apply();

		// layer masks
		TERRAIN_MASK = LayerMask.GetMask("Terrain");
		SELECTABLE_MASK = LayerMask.GetMask("Unit", "Player Avatar", "Grave");


		RTSCamera = Camera.allCameras.Single(camera => camera.name == "RTSCamera");
		
		// apparent speed must be maintained! http://physics.stackexchange.com/a/188075
		// actual speed = distance * apparent speed
		cameraPanSpeed = RTSCamera.orthographicSize * PAN_SPEED_RATIO;

		yellowCircle = GameObject.Find("yellowCircle").transform;
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
			float newSize = RTSCamera.orthographicSize + ev.delta.y * 0.25f;

			//limit y position between 6 and 50
			if (newSize > 6 && newSize < 23) {
				RTSCamera.orthographicSize = newSize;

				// apparent speed must be maintained! http://physics.stackexchange.com/a/188075
				// actual speed = distance * apparent speed
				cameraPanSpeed = newSize * PAN_SPEED_RATIO;
			}
		}
	}

	/// highlight = mouse is hovering over unit / unit is inside selection box
	/// selection = unit is selected and under player control
	void Update() {
		Ray ray = RTSCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitData;

		RTSEntity underMouse = null;
		Ghost unit = null;
		PlayerHealth VRPlayer = null;
		Grave grave = null;

		//perform raycast
		if (Physics.Raycast(ray, out hitData, 10000f, SELECTABLE_MASK)) {
			unit = hitData.transform.GetComponentInParent<Ghost>();
			underMouse = unit;

			if (unit == null) {
				VRPlayer = hitData.transform.GetComponent<PlayerHealth>();
				underMouse = VRPlayer;

				if (VRPlayer == null) {
					grave = hitData.transform.GetComponent<Grave>();
					underMouse = grave;
				}
			}
		}

		//handle highlighting (when not dragging mouse)
		if (!Input.GetMouseButton(0)) {
			//if object under mouse has changed
			if (hoverHighlight != underMouse) {
				//unhighlight it if not selected
				setHoverHighlightNull();
			}

			//if mouse is hovering over object and nothing is highlighted
			if (underMouse != null && hoverHighlight == null) {
				//highlight this object
				hoverHighlight = underMouse;
				underMouse.setHighlight(true);
			}
		}

		//left mouse click
		if (Input.GetMouseButtonDown(0)) {
			//save mouse position
			mouseDragOrigin = Input.mousePosition;

			if (underMouse is RTSEnemy) {
				attackTarget(underMouse);
			} else {
				deselectAll();

				//if hovering over unit
				if (unit != null) {
					//select only this unit
					selectedUnits.Add(unit);
					unit.setHighlight(true);
					hoverHighlight = null;
				}
			}
		}

		selectByNumKeys();

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
					unit0.setHighlight(true);
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
			if (underMouse is RTSEnemy) {
				attackTarget(underMouse);
			} else if (Physics.Raycast(ray, out hitData, 10000f, TERRAIN_MASK)) {
				// place the destination marker just above raycast hit, parellel to normal
				yellowCircle.position = hitData.point + hitData.normal * 0.1f;
				yellowCircle.forward = hitData.normal;

				foreach (Ghost unit0 in selectedUnits) {
					unit0.SetDestination(hitData.point);
				}
			}
		}

		//pan camera when mouse is at screen edge

		Vector3 panVector = Vector2.zero;

		if (//Input.mousePosition.x <= 0 || 
			Input.GetKey(KeyCode.A)) {
			panVector += -Vector3.right;
		} else if (//Input.mousePosition.x >= Screen.width-1 || 
			Input.GetKey(KeyCode.D)) {
			panVector += Vector3.right;
		}

		if (//Input.mousePosition.y <= 0 || 
			Input.GetKey(KeyCode.S)) {
			panVector += -Vector3.forward;
		} else if (//Input.mousePosition.y >= Screen.height-1 || 
			Input.GetKey(KeyCode.W)) {
			panVector += Vector3.forward;
		}

		RTSCamera.transform.position += panVector.normalized * cameraPanSpeed * Time.deltaTime;

		//prevent camera from leaving edges of map

		panVector = Vector3.zero;

		Rect groundRect = calcFrustrumGroundIntersection();
		
		//left
		if (groundRect.xMin < WORLD_BOUNDS.xMin) {
			Debug.Log("left");
			panVector.x += WORLD_BOUNDS.xMin - groundRect.xMin;
		}
		//bottom
		if (groundRect.yMin < WORLD_BOUNDS.yMin) {
			Debug.Log("bottom");
			panVector.z += WORLD_BOUNDS.yMin - groundRect.yMin;
		}
		//right
		if (groundRect.xMax > WORLD_BOUNDS.xMax) {
			Debug.Log("right");
			panVector.x += WORLD_BOUNDS.xMax - groundRect.xMax;
		}
		//top
		if (groundRect.yMax > WORLD_BOUNDS.yMax) {
			Debug.Log("top");
			panVector.z += WORLD_BOUNDS.yMax - groundRect.yMax;
		}

		RTSCamera.transform.position += panVector;

		Debug.Log(groundRect+", "+WORLD_BOUNDS);
	}



	// -------------- HELPER FUNCTIONS ---------------

	void selectByNumKeys() {
		GameObject[] ghosts = GameObject.FindGameObjectsWithTag(UNIT_TAG);
		
		//numbers 1-9
		for (int i = 1; i <= 9 && i <= ghosts.Length; i++) {
			KeyCode keyCode = (KeyCode)((int)KeyCode.Alpha0 + i);

			if (Input.GetKey(keyCode)) {
				deselectAll();
				Ghost unit = ghosts[i-1].GetComponent<Ghost>();
				selectedUnits.Add(unit);
				unit.setHighlight(true);
			}
		}
	}

	Rect calcFrustrumGroundIntersection() {
		//shoot rays out of corners of camera
		Ray bottomLeftRay = RTSCamera.ViewportPointToRay(new Vector3(0, 0, 0));
		Ray topRightRay = RTSCamera.ViewportPointToRay(new Vector3(1, 1, 0));
		
		//plane aligned with ground
		Plane groundPlane = new Plane(Vector3.up, 0);

		//get distance from ray origin to ground plane
		float bottomLeftDistance;
		float topRightDistance;
		groundPlane.Raycast(bottomLeftRay, out bottomLeftDistance);
		groundPlane.Raycast(topRightRay, out topRightDistance);

		//get points of intersection
		Vector3 bottomLeftPoint = bottomLeftRay.GetPoint(bottomLeftDistance);
		Vector3 topRightPoint = topRightRay.GetPoint(topRightDistance);
		
		//replace y (up-down direction) with z (left-right direction)
		bottomLeftPoint.y = bottomLeftPoint.z;
		topRightPoint.y = topRightPoint.z;

		//origin (upper left corner), size
		return new Rect(bottomLeftPoint, topRightPoint - bottomLeftPoint);
	}

	void attackTarget(RTSEntity target) {
		// place the destination marker above the floor
		Vector3 markerPos = target.transform.position;
		markerPos.y = 0.1f;
		yellowCircle.position = markerPos;
				
		// face upwards
		yellowCircle.forward = transform.up;
				
		foreach (Ghost unit in selectedUnits) {
			unit.SetTarget(target);
		}
	}

	void setHoverHighlightNull() {
		//if unit is highlighted and not selected
		if (hoverHighlight != null) {
			if (!(hoverHighlight is Ghost) || !selectedUnits.Contains((Ghost)hoverHighlight)) {
				hoverHighlight.setHighlight(false);
			}
		}
		hoverHighlight = null;
	}

	void unhighlightAll() {
		//remove highlight
		foreach (Ghost unit in highlightedUnits) {
			unit.setHighlight(false);
		}

		//deselect all
		highlightedUnits.Clear();
	}


	void deselectAll() {
		//remove highlight
		foreach (Ghost unit in selectedUnits) {
			unit.setHighlight(false);
		}

		//deselect all
		selectedUnits.Clear();
	}
}
