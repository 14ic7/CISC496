using UnityEngine;

public interface Damageable {
	MonoBehaviour script { get; }
	void Damage(float damage);
}
