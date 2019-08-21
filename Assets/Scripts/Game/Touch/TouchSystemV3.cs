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
        gc.Player.GetComponent<ColorableComponent>().SetColor();
        if (!GameSystem.HasMatch()) {
            BaseObject.AddComponent<LossComponent>();
        }

        GameObject.Destroy(GameSystem.GetExistingMatch());
        GameObject.Destroy(TutorialSystem.GetExistingMatch());
    }
}