using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections.Generic;
using UnityEngine.UI;

public class Ghost : RTSEntity {
	public static readonly Color LIGHT_BLUE = new Color(0, 0.6f, 1, 0.32f);

	public RTSEntity enemy;

	const float FULL_HEALTH = 10;
	readonly Color PURPLE = new Color32(144, 66, 244, 82); // purple = stunned

	float health = FULL_HEALTH;
	bool attacking = false;

	Material material;
	AICharacterControl AIScript;
	Animator animator;
	HealthBar[] healthBars;
	



	// --------------- PRIVATE FUNCTIONS ---------------

	void Start () {
		//play spawn sound
		GetComponent<AudioSource>().Play();

		//Get child components
		Transform child = transform.GetChild(0);
		material = child.GetComponent<MeshRenderer>().materials[1];
		animator = child.GetComponent<Animator>();
		
		//Get worldspace canvas healthbars
		healthBars = GetComponentsInChildren<HealthBar>();

		//Get navigation scripts
		AIScript = GetComponent<AICharacterControl>();
		SetDestination(transform.position);
	}

	void setAttacking(bool value) {
		attacking = value;
		animator.SetBool("attack", value);
	}



	// --------------- PUBLIC FUNTIONS ---------------

	// public setter for navmesh target. Do not use internally! Use AIScript.SetDestination instead
	public void SetDestination(Vector3 destination) {
		if (attacking) {
			setAttacking(false);
			enemy = null;
		}
		AIScript.SetDestination(destination);
	}

	public void SetTarget(RTSEntity target) {
		enemy = target;
		AIScript.target = target.transform;
	}

	public void Attack() {
		Debug.Log("attacking target: "+enemy.name);

		setAttacking(true);
		
		// stop moving
		AIScript.SetDestination(transform.position);
	}

	public void stun() {
		Debug.Log("stun "+name);
		animator.SetBool("attack", false);

		AIScript.Pause();
		setHighlight(PURPLE);
	}

	public void unstun() {
		Debug.Log("unstun "+name);
		animator.SetBool("attack", attacking);

		AIScript.Resume();
		setHighlight(false);
	}

	public void hurtEnemy() {
		if(enemy != null) {
			enemy.Damage(10);
		} else {
			Debug.Log("stop attacking");

			// stop attacking, remove enemy reference
			setAttacking(false);
			enemy = null;
			
			// Stop moving
			SetDestination(transform.position);
		}
	}



	// --------------- OVERRIDE FUNCTIONS ---------------

	public override void Damage(float damage) {
		health -= damage;
		if (health <= 0) {
			Destroy(gameObject);

			// minus 1 since this object isn't destroyed until the end of the frame
			int ghostsRemaining = GameObject.FindGameObjectsWithTag(RTSControls.UNIT_TAG).Length - 1;

			GameObject.Find("Ghosts Killed").GetComponent<Text>().text = ghostsRemaining.ToString();

			// if all units dead, show win/lose screens
			if (ghostsRemaining <= 0) {
				GameObject.Find("RTSUI").GetComponent<RTSUI>().lose();
				GameObject.Find("You Win (VR)").GetComponent<GameOver>().enabled = true;
			}
		} else {
			foreach (HealthBar bar in healthBars) {
				bar.health = health/FULL_HEALTH;
			}
		}
	}
	
	//set coloured outline on shader
	public override void setHighlight(Color colour) {
		material.color = colour;
	}
	public override void setHighlight(bool value) {
		if (value) {
			material.color = LIGHT_BLUE;
		} else {
			material.color = Color.white;
		}
	}
}
