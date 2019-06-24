﻿using System.Collections;
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
            GameController gc = (Controller() as GameController);
            gc.score++;
            gc.matchStreak++;
            if (gc.matchStreak > gc.maxMatchStreak) {
                gc.maxMatchStreak = gc.matchStreak;
            }
            gc.missStreak = 0;
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
        Color currentColor = gc.Target.GetComponent<ColorableComponent>().color;
        Color randomColor = RandomColor();

        gc.background.GetComponent<SpriteRenderer>().color = currentColor == Color.clear ? RandomColor() : currentColor;
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

        if (gc.firstAdLevel == round) {
            gc.gameObject.AddComponent<AdComponent>();
        }
    }

    private Color RandomColor() {
        return new Color(Utils.RandomFloat(1.0f), Utils.RandomFloat(1.0f), Utils.RandomFloat(1.0f));
    }
}
