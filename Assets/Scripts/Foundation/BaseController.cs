using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:
//   hide all components when disabled?

public class BaseController : MonoBehaviour {
	public List<BaseSystem> Systems = new List<BaseSystem>();

	private bool _enabled;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!_enabled) {
			return;
		}

		foreach (var s in Systems) {
			s.Update();
		}
	}

	public void Enable() {
		StartSystems();
		_enabled = true;
	}

	public void Disable() {
		StopSystems();
		_enabled = false;
	}

	public void TogglePause() {
		_enabled = !_enabled;
	}

	public void AddSystem(BaseSystem s) {
		s.BaseObject = gameObject;
		Systems.Add(s);
	}

	public void RemoveSystem(BaseSystem s) {
		s.BaseObject = null;
		Systems.Remove(s);
	}

	protected void StartSystems() {
		foreach (var s in Systems) {
			s.Start();
		}
	}

	protected void StopSystems() {
		foreach (var s in Systems) {
			s.Stop();
		}
	}

	protected T GetSystem<T>() where T : BaseSystem {
		foreach (BaseSystem s in this.Systems) {
			if (s is T) {
				return s as T;
			}
		}

		return null;
	}

	public Coroutine HandleCoroutine(IEnumerator routine) {
		return StartCoroutine(routine);
	}
}
