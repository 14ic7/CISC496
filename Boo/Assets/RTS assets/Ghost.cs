using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections.Generic;
using UnityEngine.UI;

public class Ghost : RTSEntity {
	public static readonly Color LIGHT_BLUE = new Color(0, 0.6f, 1, 0.32f);
	readonly Color32 YELLOW = new Color32(255, 242, 0, 1);

	public RTSEntity enemy;

	const float FULL_HEALTH = 10;
	readonly Color PURPLE = new Color32(144, 66, 244, 82); // purple = stunned

	float health = FULL_HEALTH;
	bool attacking = false;
	Timer damageTimer = new Timer(0.07f);

	Transform child;
	Transform vacuum;
	Material material;
	AICharacterControl AIScript;
	Animator animator;
	HealthBar[] healthBars;
	
	AudioClip spawnSFX;
	AudioClip deathSFX;
	AudioClip attackSFX;


	// --------------- PRIVATE FUNCTIONS ---------------

	void Start () {
		vacuum = GameObject.Find("Gun").transform;

		//play spawn sound
		spawnSFX = (AudioClip) Resources.Load("Audio/sfx-ghost-spawn");
		GetComponent<AudioSource>().PlayOneShot(spawnSFX);

		//Get child components
		child = transform.GetChild(0);
		material = child.GetComponent<MeshRenderer>().materials[1];
		animator = child.GetComponent<Animator>();
		
		//Get worldspace canvas healthbars
		healthBars = GetComponentsInChildren<HealthBar>();

		//Get navigation scripts
		AIScript = GetComponent<AICharacterControl>();
		SetDestination(transform.position);

		attackSFX = (AudioClip)Resources.Load ("Audio/sfx-ghost-attack");
	}

	void Update() {
		if (health > 0) {
			damageTimer.UpdateTimer();

			if (!damageTimer.IsRunning()) {
				animator.SetBool("damaged", false);
			}
		} else {
			child.position = Vector3.Lerp(child.position, vacuum.position, deathSFX.length * Time.deltaTime);
		}
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
		GetComponent<AudioSource> ().clip = attackSFX;
		GetComponent<AudioSource> ().Play ();
		
		// stop moving
		AIScript.SetDestination(transform.position);
	}

	public void stun() {
		animator.SetBool("attack", false);
		GetComponent<AudioSource> ().Stop ();

		AIScript.Pause();
		setHighlight(PURPLE);
	}

	public void unstun() {
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
			GetComponent<AudioSource>().Stop ();
			enemy = null;
			
			// Stop moving
			SetDestination(transform.position);
		}
	}



	// --------------- OVERRIDE FUNCTIONS ---------------

	public override void Damage(float damage) {
		health -= damage;

		if (health <= 0) {
			deathSFX = (AudioClip)Resources.Load("Audio/sfx-ghost-dying");
			Debug.Log(deathSFX);
			GetComponent<AudioSource>().PlayOneShot(deathSFX, 0.1f);
			Destroy(gameObject, deathSFX.length);

			// minus 1 since this object isn't destroyed until the end of the frame
			int ghostsRemaining = GameObject.FindGameObjectsWithTag(RTSControls.UNIT_TAG).Length - 1;

			GameObject.Find("Ghosts Killed").GetComponent<Text>().text = ghostsRemaining.ToString();

			// if all units dead, show win/lose screens
			if (ghostsRemaining <= 0) {
				GameObject.Find("WinLoseScreen").GetComponent<RTSWinLose>().lose();
				GameObject.Find("You Win (VR)").GetComponent<GameOver>().enabled = true;
			}
		} else {
			animator.SetBool("damaged", true);
			damageTimer.ResetTimer();

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
