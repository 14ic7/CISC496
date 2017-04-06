using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GhostDeath : MonoBehaviour {
	public string TARGET_NAME;

	Vector3 startPos;
	Vector3 startScale;
	Transform target;
	float time;

	void OnEnable () {
		startPos = transform.position;
		startScale = transform.localScale;
		target = GameObject.Find(TARGET_NAME).transform;
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (time < 1) {
			time += Time.deltaTime;
			transform.position = Vector3.Lerp(startPos, target.position, time);
			transform.localScale = Vector3.Lerp(startScale, Vector3.zero, time);
		} else {
			int ghostsRemaining = GameObject.FindGameObjectsWithTag(RTSControls.UNIT_TAG).Length - 1;

			GameObject.Find("Ghosts Killed").GetComponent<Text>().text = ghostsRemaining.ToString();

			// if all units dead, show win/lose screens
			if (ghostsRemaining <= 0) {
				GameObject.Find("WinLoseScreen").GetComponent<RTSWinLose>().lose();
				GameObject.Find("You Win (VR)").GetComponent<GameOver>().enabled = true;
			}

			// kill yourself
			Destroy(gameObject);
		}
	}
}
