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

            if (gc.matchStreak > 0) {
                if (gc.matchStreak % gc.matchStreakForStock == 0) {
                    gc.Lives++;
                }

                if (gc.matchStreak % 2 == 0) {
                    int newNumberOfButtons = gc.numberOfButtons + 2;
                    for (int i = gc.numberOfButtons; i < newNumberOfButtons; i++) {
                        AnimationComponent.Animate(
                            gc.ColorButtons[i],
                            "isOn",
                            true,
                            null,
                            "anim_appear"
                        );
                    }
                    gc.numberOfButtons = Mathf.Min(newNumberOfButtons, gc.ColorButtons.Count);
                }

                if (gc.matchStreak > 1) {
                    gc.difficulty += 1;
                }
            }


            if (gc.missStreak > 0) {
                if (gc.missStreak % 2 == 0) {
                    int newNumberOfButtons = gc.numberOfButtons - 2;
                    for (int i = gc.numberOfButtons - 1; i > newNumberOfButtons - 1; i--) {
                        AnimationComponent.Animate(
                            gc.ColorButtons[i],
                            "isOn",
                            false,
                            null,
                            "anim_idle"
                        );
                    }
                    gc.numberOfButtons = Mathf.Max(gc.numberOfButtons - 2, 3);
                }

                if (gc.missStreak % 2 == 0) {
                    gc.difficulty -= 1;
                }
            }
            gc.missStreak = 0;
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
