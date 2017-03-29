using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class HealthBar : MonoBehaviour {
	public string CAMERA_NAME;

	Image healthBar;
	new Camera camera;

	void Start () {
		healthBar = GetComponent<Image>();

		camera = Camera.allCameras.Single(cam => cam.name == CAMERA_NAME);
	}
	
	void Update () {
		if (healthBar.canvas != null) {
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
