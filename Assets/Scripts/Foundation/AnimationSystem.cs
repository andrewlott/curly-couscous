using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimationSystem : BaseSystem {

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(AnimationComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(AnimationComponent), this);
	}

	public override void Update() {
		List<BaseComponent> animationComponents = Pool.Instance.ComponentsForType(typeof(AnimationComponent)).ToList();
		foreach (AnimationComponent ac in animationComponents) {
			if (ac.CallbackState == null || IsAnimationComponentComplete(ac)) {
				if (ac.Callback != null) {
					ac.Callback(ac.gameObject);
				}
				if (ac != null) {
					GameObject.Destroy(ac);
				}
			}
		}
	}

	private bool IsAnimationComponentComplete(AnimationComponent ac) {
		Animator a = ac.gameObject.GetComponent<Animator>();
		return a.GetCurrentAnimatorStateInfo(0).IsName(ac.CallbackState);
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is AnimationComponent) {
			AnimationComponent ac = c as AnimationComponent;
			Animator a = ac.gameObject.GetComponent<Animator>();
			a.SetTrigger(ac.Trigger);
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is AnimationComponent) {
		}
	}
}
