using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : BaseSystem {
	private static int Round = 0;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
	}

	public override void Update() {
		if (Round == 0) {
			Round++;
			Reset(Round);
		}

		List<BaseComponent> matchableComponents = Pool.Instance.ComponentsForType(typeof(MatchableComponent));
		MatchableComponent target = null;
		bool hasMatch = true;
		foreach (MatchableComponent mac in matchableComponents) {
			ColorableComponent cac = mac.GetComponent<ColorableComponent>();

			if (target == null) {
				target = mac;
				continue;
			}

			Color targetColor = target.GetComponent<ColorableComponent>().color;
			if (targetColor != cac.color) {
				hasMatch = false;
			}
		}

		if (this.GetExistingMatch() == null && target && hasMatch) {
			GameObject.Find("GameController").AddComponent<MatchComponent>();
		}
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is MatchComponent) {
			this.OnMatch(c.gameObject);
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is MatchComponent) {
			
		}
	}

	private MatchComponent GetExistingMatch() {
		return Pool.Instance.ComponentForType(typeof(MatchComponent)) as MatchComponent;
	}

	private void OnMatch(GameObject g) {
		Round++;
		GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
		AnimateCorrect(gc.playerBox, null);
		AnimateCorrect(gc.targetBox, ClearAnimationCallback);
		foreach (GameObject cube in gc.colorButtons) {
			AnimateCorrect(cube, null);
		}

	}

	private void AnimateCorrect(GameObject g, AnimationComponent.CallbackFunction callback) {
		AnimationComponent ac = g.AddComponent<AnimationComponent>();
		ac.Trigger = "isCorrect";
		if (callback != null) {
			ac.Callback = callback;
		}
		ac.CallbackState = "anim_correct";
	}

	private void AnimateNextRound(GameObject g, AnimationComponent.CallbackFunction callback) {
		AnimationComponent ac = g.AddComponent<AnimationComponent>();
//		a.SetBool(ac.Trigger, true); // hack around lazy non-trigger
		ac.Trigger = "isNextRound";
		if (callback != null) {
			ac.Callback = callback;
			ac.CallbackState = "anim_idle";
		}
	}

	public void ClearAnimationCallback(GameObject g) {
		Reset(Round);
		GameObject.Destroy(this.GetExistingMatch());
//		ColorComponent gc = g.GetComponent<ColorComponent>();
//		GameObject.Destroy(gc);
//		gc = g.AddComponent<ColorComponent>();
//		gc.color = ColorType.None;
//		gc.shouldRandomize = false;
	}

	private void Reset(int round) {
		GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
		gc.background.GetComponent<SpriteRenderer>().color = gc.targetBox.GetComponent<ColorableComponent>().color;

		Color randomColor = new Color(Utils.RandomFloat(1.0f), Utils.RandomFloat(1.0f), Utils.RandomFloat(1.0f));
		gc.targetBox.GetComponent<ColorableComponent>().color = randomColor;
		gc.playerBox.GetComponent<ColorableComponent>().color = Color.clear;

		float offset = 0.2f;
		int index = 0;
		int lowerBound = Mathf.Max(0, gc.colorButtons.Count / 2 - round);
		int upperBound = Mathf.Min(gc.colorButtons.Count - 1, gc.colorButtons.Count / 2 + round);
		int targetIndex = lowerBound + Utils.RandomInt(upperBound - lowerBound);
		foreach (GameObject go in gc.colorButtons) {
			Color buttonColor = Color.clear;
			if (index == targetIndex) {
				buttonColor = randomColor;
			} else if (index >= lowerBound && index <= upperBound) {
				float r =  randomColor.r - (offset / 2.0f) + Utils.RandomFloat(offset);
				float g =  randomColor.g - (offset / 2.0f) + Utils.RandomFloat(offset);
				float b =  randomColor.b - (offset / 2.0f) + Utils.RandomFloat(offset);

				buttonColor = new Color(r, g, b);
			}
				
			ColorableComponent cac = go.GetComponent<ColorableComponent>();
			cac.color = buttonColor;
			index++;
		}
	}
}
