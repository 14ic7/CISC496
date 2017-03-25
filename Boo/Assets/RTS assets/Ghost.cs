using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Ghost : MonoBehaviour, Selectable {
	static int ghostsKilled;
	public static readonly Color LIGHT_BLUE = new Color(0, 0.6f, 1, 0.32f);

	public AudioSource sfx;

	const string RTS_UI_NAME = "RTSUI";
	const float FULL_HEALTH = 10;
	readonly Color PURPLE = new Color32(144, 66, 244, 82); // purple = stunned
	readonly Color BLOOD_RED = new Color(1, 0, 0, 0.49f);

	float health = FULL_HEALTH;
	bool attacking = false;
	bool stunned = false;

	Material material;
	AICharacterControl AIScript;
	NavMeshAgent navMeshAgent;
	Animator animator;
	HealthBar RTSHealthBar;
	HealthBar VRHealthBar;

	void Start () {
		ghostsKilled = 0;
		
		sfx.Play();

		Transform child = transform.GetChild(0);
		material = child.GetComponent<MeshRenderer>().materials[1];
		navMeshAgent = GetComponent<NavMeshAgent>();
		animator = child.GetComponent<Animator>();
		RTSHealthBar = transform.GetChild(1).GetChild(0).GetComponent<HealthBar>();
		VRHealthBar = transform.GetChild(2).GetChild(0).GetComponent<HealthBar>();

		AIScript = GetComponent<AICharacterControl>();
		SetDestination(transform.position);
	}

	//set navmesh target position
	public void SetDestination(Vector3 destination) {
		if (attacking) {
			setAttacking(false);
			navMeshAgent.Resume();
		}

		AIScript.SetDestination(destination);
	}
	//set navmesh target
	public void SetTarget(Transform target) {
		AIScript.target = target;
	}

	public void Attack(Collision collision) {
		//if collided with VR player and VR player is selected
		if (AIScript.target == collision.transform) {
			navMeshAgent.Stop();
			setAttacking(true);
		}
	}

	public void stun() {
		Debug.Log("stun "+name);
		animator.SetBool("attack", false);

		navMeshAgent.Stop();
		setHighlight(PURPLE);
	}

	public void unstun() {
		Debug.Log("unstun "+name);
		animator.SetBool("attack", attacking);

		navMeshAgent.Resume();
		setHighlight(false);
	}

	public void hurt(float damage) {
		health -= damage;
		if (health <= 0) {
			Destroy(gameObject);
			GameObject.Find("Ghosts Killed").GetComponent<Text>().text = (ghostsKilled + 1).ToString();
			ghostsKilled++;

			//if all units dead, display "You Lose" message
			//check length <= 1 since this object isn't destroyed until the end of the frame
			if (GameObject.FindGameObjectsWithTag(RTSControls.UNIT_TAG).Length <= 1) {
				GameObject.Find(RTS_UI_NAME).GetComponent<Canvas>().enabled = true;
				GameObject.Find ("You Win (VR)").GetComponent<GameOver> ().enabled = true;
			}
		} else {
			RTSHealthBar.health = health/FULL_HEALTH;
			VRHealthBar.health = health/FULL_HEALTH;
		}
	}
	
	//set coloured outline on shader
	public void setHighlight(Color colour) {
		material.color = colour;
	}
	public void setHighlight(bool value) {
		if (value) {
			material.color = LIGHT_BLUE;
		} else {
			material.color = Color.white;
		}
	}

	void setAttacking(bool value) {
		attacking = value;
		animator.SetBool("attack", value);
	}
}
