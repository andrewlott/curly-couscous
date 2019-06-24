using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreakSystem : BaseSystem {
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
            gc.matchStreak++;
            if (gc.matchStreak > gc.maxMatchStreak) {
                gc.maxMatchStreak = gc.matchStreak;
            }
            gc.missStreak = 0;

            // TODO: Pop UI
            if (gc.matchStreak > 0 && gc.matchStreak % gc.matchStreakForStock == 0) {
                gc.Lives++;
            }
        } else if (c is LossComponent) {
            GameController gc = (Controller() as GameController);
            gc.missStreak++;
            gc.matchStreak = 0;
        }
    }

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is LossComponent) {

   		}
	}
}
