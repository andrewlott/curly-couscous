using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoringSystem : BaseSystem {
	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(ColorComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(ColorComponent), this);
	}

	public override void Update() {

	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is ColorComponent) {
			ColorComponent cc = c as ColorComponent;
			ColorableComponent cac = c.GetComponent<ColorableComponent>();
			if (cac) {
				MeshRenderer mr = cac.renderer;
				if (!mr) {
					mr = cc.GetComponent<MeshRenderer>();
				}
				mr.material.color = new Color(Utils.RandomFloat(1.0f), Utils.RandomFloat(1.0f), Utils.RandomFloat(1.0f)); //cc.color;
			}

			GameObject.Destroy(c);
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is ColorComponent) {
			
		}
	}
}
