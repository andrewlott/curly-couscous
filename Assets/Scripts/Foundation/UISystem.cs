using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : BaseSystem {
	int score = 0;
	bool scoreDirty = true;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
	}

	public override void Update() {
		GameController gc = Controller() as GameController;
		if (scoreDirty) {
			gc.scoreText.text = string.Format("Score: {0}", this.score);
			scoreDirty = false;
		}

		int min = ((int) gc.totalTime) / 60;
		int sec = ((int) gc.totalTime) % 60;
		gc.timerText.text = string.Format("{0:D2}:{1:D2}", Mathf.Max(min, 0), Mathf.Max(sec, 0));
		gc.totalTime -= Time.deltaTime;
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is MatchComponent) {

		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is MatchComponent) {
			GameController gc = Controller() as GameController;

			this.score++;
			this.scoreDirty = true;

			gc.totalTime += gc.bonusTime;
		}
	}
}
