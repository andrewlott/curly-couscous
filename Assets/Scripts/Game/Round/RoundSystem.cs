using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : BaseSystem {
	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
	}

	public override void Update() {
		GameController gc = (Controller() as GameController);
		if (gc.initializeGame) {
			Reset(gc.round);
			gc.initializeGame = false;
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
        Reset((Controller() as GameController).round++);
	}

	private void Reset(int round) {
        GameController gc = GameController.Instance;
        Color currentColor = gc.Target.GetComponent<ColorableComponent>().color;
        Color randomColor = RandomColor();
        Color bgColor = gc.initializeGame && currentColor == Color.clear ? RandomColor() : currentColor;
        gc.background.GetComponent<SpriteRenderer>().color = bgColor;
        gc.Target.GetComponent<ColorableComponent>().color = randomColor;
        gc.Player.GetComponent<ColorableComponent>().color = bgColor;

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

            if (buttonColor != Color.clear) {
                ColorableComponent cac = go.GetComponent<ColorableComponent>();
                cac.color = buttonColor;
            }
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
