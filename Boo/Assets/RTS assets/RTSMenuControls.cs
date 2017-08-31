using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RTSMenuControls : MonoBehaviour {
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

	private GameObject reaper;
	private GameObject playGrave;
	private GameObject quitGrave;

	private Material coatMaterial;
	private Color coatColour;

	private Material playGraveMaterial;
	private Material quitGraveMaterial;
	private Color graveColour;

	// --------------- INITIALIZATION ---------------

	void Start()
	{
		RTSCamera = Camera.allCameras.Single(camera => camera.name == "RTSCamera");

		reaper = GameObject.Find("Tutorial RTS");
		coatMaterial = reaper.transform.GetChild(1).GetComponent<MeshRenderer>().materials.Single(material => material.name == "black (Instance)");
		coatColour = coatMaterial.color;

		playGrave = GameObject.Find("Play RTS");
		playGraveMaterial = playGrave.GetComponent<MeshRenderer>().materials.Single(material => material.name == "Base (Instance)");
		graveColour = playGraveMaterial.color;

		quitGrave = GameObject.Find("Quit RTS");
		quitGraveMaterial = quitGrave.GetComponent<MeshRenderer>().materials.Single(material => material.name == "Base (Instance)");
	}


	/*
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
	*/

	/// highlight = mouse is hovering over unit / unit is inside selection box
	/// selection = unit is selected and under player control
	void Update() {
		Ray ray = RTSCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitData;

		//perform raycast
		Physics.Raycast(ray, out hitData);

		GameObject hitObject = hitData.transform == null ? null : hitData.transform.gameObject;
		//Debug.Log(hitObject == null ? null : hitObject.name);

		coatMaterial.color = (hitObject == reaper) ? Color.white : coatColour;
		playGraveMaterial.color = (hitObject == playGrave) ? Color.white : graveColour;
		quitGraveMaterial.color = (hitObject == quitGrave) ? Color.white : graveColour;

		//left mouse click
		if (Input.GetMouseButtonDown(0)) {
			if (hitObject == reaper)
			{
				// add VR tutorial to the current scene
				Debug.Log("loadScene");
				SceneManager.LoadScene("RTS Tutorial", LoadSceneMode.Additive);
			}
			else if (hitObject == playGrave)
			{
				VRTitle VRHandler = GameObject.Find("OVRCameraRig").GetComponent<VRTitle>();
				VRHandler.setRTSReady();
				GameObject.Find("Explanation (RTS)").GetComponent<Text>().text = "Waiting for the VR player to ready up...";
			}
			else if (hitObject == quitGrave)
			{
				Application.Quit();
			}
		}

		/*
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
				yellowCircle.position = hitData.point + hitData.normal * 0.01f;
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
			panVector.x += WORLD_BOUNDS.xMin - groundRect.xMin;
		}
		//bottom
		if (groundRect.yMin < WORLD_BOUNDS.yMin) {
			panVector.z += WORLD_BOUNDS.yMin - groundRect.yMin;
		}
		//right
		if (groundRect.xMax > WORLD_BOUNDS.xMax) {
			panVector.x += WORLD_BOUNDS.xMax - groundRect.xMax;
		}
		//top
		if (groundRect.yMax > WORLD_BOUNDS.yMax) {
			panVector.z += WORLD_BOUNDS.yMax - groundRect.yMax;
		}

		RTSCamera.transform.position += panVector;
		*/
	}



	// -------------- HELPER FUNCTIONS ---------------
	/*
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
		markerPos.y = 0.01f;
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
	*/
}