using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent : MonoBehaviour {

	void Start() {
		Pool.Instance.AddComponent(this);
		ComponentStart();
	}

	public virtual void ComponentStart() {

	}

	void OnDestroy() {
		Pool.Instance.RemoveComponent(this);
	}

	public virtual void ComponentDestroy() {

	}
}
