using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchSystem : BaseSystem {
	// TODO: Do this smarter
	static bool IsAnimating = false;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(TouchComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(TouchComponent), this);
	}

	public override void Update() {

	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is TouchComponent) {
			if (!IsAnimating) {
				GameController gameController = GameController.Instance;
				ColorableComponent cac = c.gameObject.GetComponent<ColorableComponent>();
				if (cac.color != Color.clear) {
					gameController.Player.GetComponent<ColorableComponent>().color = cac.color;
					OnTouch();
				}
			}
			GameObject.Destroy(c);
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is TouchComponent) {

		}
	}

	private void OnTouch() {
		IsAnimating = true;
		GameController gc = GameController.Instance;

		string trigger = "isIncorrect";
		AnimationComponent.CallbackFunction doOnceCallback = DefaultClearAnimationCallback;
		string callbackState = "anim_incorrect";

		if (RoundSystem.HasMatch()) {
			trigger = "isCorrect";
			doOnceCallback = ClearCorrectAnimationCallback;
			callbackState = "anim_correct";
        } else {
            BaseObject.AddComponent<LossComponent>();
        }

        AnimationComponent.Animate(gc.Player, trigger, false, null, callbackState);
		AnimationComponent.Animate(gc.Target, trigger, false, doOnceCallback, callbackState);
		foreach (GameObject cube in gc.ColorButtons) {
			AnimationComponent.Animate(cube, trigger, false, null, callbackState);
		}
	}

	public void DefaultClearAnimationCallback(GameObject g) {
		IsAnimating = false;
	}

	public void ClearCorrectAnimationCallback(GameObject g) {
		IsAnimating = false;
		GameObject.Destroy(RoundSystem.GetExistingMatch());
	}
}