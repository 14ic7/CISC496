using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {
	Image healthBar;

	void Start () {
		healthBar = GetComponent<Image>();
	}
	
	void Update () {
		if (healthBar.canvas != null) {
			//match camera's orientation
			transform.rotation = healthBar.canvas.worldCamera.transform.rotation;
		}
	}

	public float health {
		set {
			healthBar.fillAmount = value;
		}
	}
}
