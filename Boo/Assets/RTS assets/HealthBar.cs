using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class HealthBar : MonoBehaviour {
	public bool useVRCamera;

	Image healthBar;
	new Camera camera;

	void Start () {
		healthBar = GetComponent<Image>();

		if (useVRCamera) {
			camera = Camera.allCameras.Single(cam => cam.name == "VR UI Camera");
		} else {
			camera = Camera.allCameras.Single(cam => cam.name == "RTSCamera");
		}
	}
	
	void Update () {
		if (useVRCamera) {
			transform.LookAt(camera.transform);
		} else {
			//match camera's orientation
			transform.rotation = camera.transform.rotation;			
		}
	}

	public float health {
		set {
			healthBar.fillAmount = value;
		}
	}
}
