using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FollowMouse : MonoBehaviour {
	Image cursor;

	void Start() {
		cursor = GetComponent<Image>();
	}

	void Update () {
		cursor.rectTransform.anchoredPosition3D = Input.mousePosition;
	}
}
