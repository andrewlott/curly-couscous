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

		if (GetExistingMatch() != null) {
			return;
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

		if (target && hasMatch) {
			GameObject.Find("GameController").AddComponent<MatchComponent>();
		}
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is MatchComponent) {

		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is MatchComponent) {
			this.OnMatch(c.gameObject);
		}
	}

	public static bool HasMatch() {
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

		return hasMatch;
	}

	public static MatchComponent GetExistingMatch() {
		return Pool.Instance.ComponentForType(typeof(MatchComponent)) as MatchComponent;
	}

	private void OnMatch(GameObject g) {
		Reset(Round++);
	}

	private void Reset(int round) {
        GameController gc = GameController.Instance;
		gc.background.GetComponent<SpriteRenderer>().color = gc.Target.GetComponent<ColorableComponent>().color;

		Color randomColor = new Color(Utils.RandomFloat(1.0f), Utils.RandomFloat(1.0f), Utils.RandomFloat(1.0f));
		gc.Target.GetComponent<ColorableComponent>().color = randomColor;
		gc.Player.GetComponent<ColorableComponent>().color = Color.clear;

		float offset = 0.2f;
		int index = 0;
        int lowerBound = 0;
        int upperBound = Mathf.Min(2 * round, gc.ColorButtons.Count);
		int targetIndex = lowerBound + Utils.RandomInt(upperBound - lowerBound);
		foreach (GameObject go in gc.ColorButtons) {
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
