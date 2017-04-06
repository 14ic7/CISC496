using UnityEngine;
using System.Collections.Generic;

public class Grave : RTSEnemy {
	const float FULL_HEALTH = 100;

	float health = FULL_HEALTH;

	Animator animator;
	List<Material> materials = new List<Material>();
	List<Color> colours = new List<Color>();

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();

		// save all child materials and colours to lists
		foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>()) {
			foreach (Material mat in rend.materials) {
				materials.Add(mat);
				colours.Add(mat.color);
			}
		}
	}
	
	public override void Damage(float damage) {
		if (health > 0) {
			health -= damage;

			if (health <= 0) {
				GetComponent<AudioSource> ().Play ();
				animator.SetBool("crumble", true);
			}
		}
	}

	// called by animator
	public void Destroy() {
		// create ghost object
		Instantiate(Resources.Load("Ghost/Ghost"), transform.parent.position, transform.parent.rotation);

		// kill your parents
		Destroy(transform.parent.gameObject);
	}

	public override void setHighlight(bool value) {
		if (value) {
			for (int i = 0; i < materials.Count; i++) {
				materials[i].color = Color.red;
			}
		} else {
			for (int i=0; i < materials.Count; i++) {
				materials[i].color = colours[i];
			}
		}
	}
	public override void setHighlight(Color colour) {
		for (int i = 0; i < materials.Count; i++) {
			materials[i].color = colour;
		}
	}
}
