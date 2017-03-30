using UnityEngine;
using System.Collections;

public abstract class RTSEntity : MonoBehaviour {
	public abstract void setHighlight(bool value);
	public abstract void setHighlight(Color colour);
	public abstract void Damage(float damage);
}
