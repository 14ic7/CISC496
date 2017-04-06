using UnityEngine;
using System.Collections;

public class Cone : MonoBehaviour {
	protected bool RaycastGhost(Ghost ghost) {
		if (ghost == null) {
			return false;
		}

		// ray from tip of the cone to the ghost
		Ray ray = new Ray(transform.position, ghost.transform.position - transform.position);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 10000f, ~LayerMask.GetMask("AttackCone", "ItemCone"))) {
			return hit.transform == ghost.transform || hit.transform == ghost.transform.parent;
		} else {
			return false;
		}
	}
}
