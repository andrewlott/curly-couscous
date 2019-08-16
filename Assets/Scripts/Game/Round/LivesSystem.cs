using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesSystem : BaseSystem {
	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(LossComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(LossComponent), this);
	}

	public override void Update() {

	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is LossComponent) {
            GameController gc = (Controller() as GameController);
            if (gc.livesMode) {
                gc.Lives--;
                if (gc.Lives <= 0) {
                    gc.HandleCoroutine(GameOver());
                }
            }
            GameObject.Destroy(c);
		    gc.PlayParticleForDifficulty();
		}
    }

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is LossComponent) {

   		}
	}

    private IEnumerator GameOver() {
        yield return new WaitForSeconds(1.25f);
        GameController gc = (Controller() as GameController);
        gc.gameObject.AddComponent<EndGameComponent>();
        gc.gameObject.AddComponent<EndTutorialComponent>();
        gc.ShowGameOver();
    }
}
