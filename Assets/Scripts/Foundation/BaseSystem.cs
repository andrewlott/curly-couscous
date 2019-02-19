using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSystem {
	public GameObject BaseObject;

	public virtual void Start() {

	}

	public virtual void Stop() {

	}

	public virtual void Update() {

	}

	public virtual void OnComponentAdded(BaseComponent c) {

	}

	public virtual void OnComponentRemoved(BaseComponent c) {

	}

	protected BaseController Controller() {
		return BaseObject.GetComponent<BaseController>();
	}
}
