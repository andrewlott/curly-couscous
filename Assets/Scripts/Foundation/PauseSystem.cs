using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : BaseSystem {

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(ColorableComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(ColorableComponent), this);
	}

	public override void Update() {

	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is PauseComponent) {
			Controller().TogglePause();
			List<BaseComponent> animationComponents = Pool.Instance.ComponentsForType(typeof(AnimatedComponent));
			foreach (AnimatedComponent ac in animationComponents) {
				Animator animator = ac.GetComponent<Animator>();
				animator.enabled = false;
			}
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is PauseComponent) {
			Controller().TogglePause();
			List<BaseComponent> animationComponents = Pool.Instance.ComponentsForType(typeof(AnimatedComponent));
			foreach (AnimatedComponent ac in animationComponents) {
				Animator animator = ac.GetComponent<Animator>();
				animator.enabled = true;
			}
		}
	}
}
