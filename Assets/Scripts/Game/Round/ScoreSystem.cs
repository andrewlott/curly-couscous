using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : BaseSystem {
	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
        Pool.Instance.AddSystemListener(typeof(LossComponent), this);
    }

    public override void Stop() {
        Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
        Pool.Instance.RemoveSystemListener(typeof(LossComponent), this);
    }

	public override void Update() {

	}

	public override void OnComponentAdded(BaseComponent c) {
        if (c is MatchComponent) {
            GameController gc = (Controller() as GameController);
            int scoreGainz = 0;

            int misses = gc.missStreak;
            if (misses == 0) {
                scoreGainz += 600;
            } else if (misses == 1) {
                scoreGainz += 400;
            } else if (misses == 2) {
                scoreGainz += 200;
            }

            float timeSinceLastMatch = Time.time - gc.lastMatchTime;
            if (timeSinceLastMatch <= 3) {
                scoreGainz += 300;
            } else if (timeSinceLastMatch <= 7) {
                scoreGainz += 150;
            }

            int matchStreak = gc.matchStreak;
            scoreGainz += ((matchStreak + 5) / 10) * 50;
            gc.Score += scoreGainz;
        } else if (c is LossComponent) {

        }
    }

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is LossComponent) {

   		}
	}
}
