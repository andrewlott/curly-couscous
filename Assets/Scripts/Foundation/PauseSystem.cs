using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : BaseSystem {

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(PauseComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(PauseComponent), this);
	}

	public override void Update() {

	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is PauseComponent) {
            GameController gc = (Controller() as GameController);
			gc.TogglePause();
			List<BaseComponent> animationComponents = Pool.Instance.ComponentsForType(typeof(AnimatedComponent));
			foreach (AnimatedComponent ac in animationComponents) {
				Animator animator = ac.GetComponent<Animator>();
				animator.enabled = false;
			}

            gc.HideCanvas(gc.gameplayCanvas);
            gc.ShowCanvas(gc.pauseCanvas);
        }
    }

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is PauseComponent) {
            GameController gc = (Controller() as GameController);
            gc.TogglePause(); 
            List<BaseComponent> animationComponents = Pool.Instance.ComponentsForType(typeof(AnimatedComponent));
			foreach (AnimatedComponent ac in animationComponents) {
				Animator animator = ac.GetComponent<Animator>();
				animator.enabled = true;
			}

            gc.HideCanvas(gc.pauseCanvas);
            gc.ShowCanvas(gc.gameplayCanvas);
        }
    }
}
