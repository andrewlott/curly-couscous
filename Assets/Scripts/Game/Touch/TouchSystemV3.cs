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
		}

		this.Animate(gc.Player, trigger, null, callbackState);
		this.Animate(gc.Target, trigger, doOnceCallback, callbackState);
		foreach (GameObject cube in gc.ColorButtons) {
			this.Animate(cube, trigger, null, callbackState);
		}
	}

	private void Animate(GameObject g, string trigger, AnimationComponent.CallbackFunction callback, string callbackState) {
		AnimationComponent ac = g.AddComponent<AnimationComponent>();
		ac.Trigger = trigger;
		ac.Callback = callback;
		ac.CallbackState = callbackState;
	}

	public void DefaultClearAnimationCallback(GameObject g) {
		IsAnimating = false;
	}

	public void ClearCorrectAnimationCallback(GameObject g) {
		IsAnimating = false;
		GameObject.Destroy(RoundSystem.GetExistingMatch());
	}
}