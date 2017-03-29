using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections.Generic;
using UnityEngine.UI;

public class Ghost : MonoBehaviour, Selectable {
	public static readonly Color LIGHT_BLUE = new Color(0, 0.6f, 1, 0.32f);

	const float FULL_HEALTH = 10;
	readonly Color PURPLE = new Color32(144, 66, 244, 82); // purple = stunned

	float health = FULL_HEALTH;
	bool attacking = false;

	Material material;
	AICharacterControl AIScript;
	Animator animator;
	HealthBar[] healthBars;
	Damageable enemy;

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

	//set navmesh target position
	public void SetDestination(Vector3 destination) {
		setAttacking(false);
		AIScript.SetDestination(destination);
	}
	//set navmesh target
	public Transform target {
		get { return AIScript.target; }
		set { AIScript.target = value; }
	}

	public void Attack(Collision collision) {
		//if collided with VR player and VR player is selected
		if (target == collision.transform) {
			enemy = (Damageable)target.GetComponent(typeof(Damageable));
			setAttacking(true);
			AIScript.Stop();
		}
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
		if(enemy.script != null) {
			enemy.Damage(10);
		} else {
			Debug.Log("stop attacking");
			setAttacking(false);
			SetDestination(transform.position);
		}
	}

	public void hurt(float damage) {
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

		if (!value) {
			AIScript.Resume();
		}
	}
}
