﻿using System.Collections;
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

		if (target && hasMatch) {
			GameObject.Find("GameController").AddComponent<MatchComponent>();
		}
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is MatchComponent) {
			Round++;
			Reset(Round);
			GameObject.Destroy(c);
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is MatchComponent) {
			
		}
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
