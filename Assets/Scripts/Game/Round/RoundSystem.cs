using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : BaseSystem {
	private int Round = 0;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
	}

	public override void Update() {
		if (Round == 0) {
            AnimateIn();
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

		if (target) {
            if (hasMatch) {
                BaseObject.AddComponent<MatchComponent>();
            }
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

    private void AnimateIn() {
        GameController gc = GameController.Instance;
        string trigger = "isOn";
        string callbackState = "anim_appear";

        AnimationComponent.Animate(gc.Player, trigger, true, null, callbackState);
        AnimationComponent.Animate(gc.Target, trigger, true, null, callbackState);
        foreach (GameObject cube in gc.ColorButtons) {
            AnimationComponent.Animate(cube, trigger, true, null, callbackState);
        }
    }

	private void Reset(int round) {
        GameController gc = GameController.Instance;
        Color currentColor = gc.Target.GetComponent<ColorableComponent>().color;
        Color randomColor = RandomColor();

        gc.background.GetComponent<SpriteRenderer>().color = currentColor == Color.clear ? RandomColor() : currentColor;
        gc.Target.GetComponent<ColorableComponent>().color = randomColor;
		gc.Player.GetComponent<ColorableComponent>().color = Color.clear;

		int index = 0;
        int lowerBound = 0;
        int upperBound = Mathf.Min(gc.numberOfButtons, gc.ColorButtons.Count);
		int targetIndex = lowerBound + Utils.RandomInt(upperBound - lowerBound);
        Debug.Log(targetIndex);
		foreach (GameObject go in gc.ColorButtons) {
			Color buttonColor = Color.clear;
			if (index == targetIndex) {
				buttonColor = randomColor;
			} else if (index >= lowerBound && index < upperBound) {
                buttonColor = SimilarColor(randomColor);
			}
				
			ColorableComponent cac = go.GetComponent<ColorableComponent>();
			cac.color = buttonColor;
			index++;
		}

        if (gc.firstAdLevel == round) {
            gc.gameObject.AddComponent<AdComponent>();
        }
        (Controller() as GameController).lastMatchTime = Time.time;
    }

    private Color RandomColor() {
        return new Color(Utils.RandomFloat(1.0f), Utils.RandomFloat(1.0f), Utils.RandomFloat(1.0f));
    }

    private Color SimilarColor(Color c) {
        GameController gc = (Controller() as GameController);
        float offset = 1.0f / gc.difficulty; // Magic equation that feels nice

        float r = Mathf.Max(Mathf.Min(c.r - (offset / 2.0f) + Utils.RandomFloat(offset), 1.0f), 0.0f);
        float g = Mathf.Max(Mathf.Min(c.g - (offset / 2.0f) + Utils.RandomFloat(offset), 1.0f), 0.0f);
        float b = Mathf.Max(Mathf.Min(c.b - (offset / 2.0f) + Utils.RandomFloat(offset), 1.0f), 0.0f);
        return new Color(r, g, b);
    }
}
