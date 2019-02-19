using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySystem : BaseSystem {

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(DestroyComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(DestroyComponent), this);
	}

	public override void Update() {
		foreach(BaseComponent c in Pool.Instance.ComponentsForType(typeof(DestroyComponent))) {
			GameObject.Destroy(c.gameObject);
		}
	}
}
