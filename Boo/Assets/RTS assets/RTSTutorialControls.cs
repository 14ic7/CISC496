using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RTSTutorialControls : MonoBehaviour {
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
		RTSCamera = Camera.allCameras.Single(camera => camera.name == "RTSCamera2");
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
	void Update()
	{
		Ray ray = RTSCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitData;

		//perform raycast
		Physics.Raycast(ray, out hitData);

		GameObject hitObject = hitData.transform == null ? null : hitData.transform.gameObject;
		//Debug.Log(hitObject == null ? null : hitObject.name);

		//left mouse click
		if (Input.GetMouseButtonDown(0) && hitObject != null && hitObject.GetComponent<Button>() != null)
		{
			// add VR tutorial to the current scene
			Debug.Log("unloadScene");
			SceneManager.UnloadSceneAsync(gameObject.scene.name);
		}
	}
}