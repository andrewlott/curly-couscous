using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchSystem : BaseSystem {
	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(TouchComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(TouchComponent), this);
	}

	public override void Update() {

	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is TouchComponent) {
			GameController gameController = GameController.Instance;
			ColorableComponent cac = c.gameObject.GetComponent<ColorableComponent>();
			if (cac.color != Color.clear) {
				gameController.Player.GetComponent<ColorableComponent>().color = cac.color;
				OnTouch();
			}
			GameObject.Destroy(c);
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is TouchComponent) {

		}
	}

	private void OnTouch() {
        if (!GameSystem.isPlaying && !TutorialSystem.isTutorialPlaying) {
            return;
        }
		GameController gc = GameController.Instance;
        string trigger = "isIncorrect";
        if (GameSystem.HasMatch()) {
            trigger = "isCorrect";
        } else {
            BaseObject.AddComponent<LossComponent>();
        }
		TriggerAnimation(gc.Player, trigger);
		TriggerAnimation(gc.Target, trigger);
        foreach (GameObject cube in gc.ColorButtons) {
			TriggerAnimation(cube, trigger);
        }
        // Cubes will all unset the bool via animation behaviour


        if (GameSystem.isPlaying) {
            GameObject.Destroy(GameSystem.GetExistingMatch());
        }
        if (TutorialSystem.isTutorialPlaying) {
            GameObject.Destroy(TutorialSystem.GetExistingMatch());
        }
    }

    private void TriggerAnimation(GameObject g, string trigger) {
		Controller().HandleWaitAndDo(Utils.RandomFloat(0.0f), () => {
			g.GetComponent<Animator>().SetBool(trigger, true);
		});
	}
}