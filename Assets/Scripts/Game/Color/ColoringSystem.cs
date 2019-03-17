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
		List<BaseComponent> colorableComponents = Pool.Instance.ComponentsForType(typeof(ColorableComponent));
		foreach (ColorableComponent cac in colorableComponents) {
			if (cac.renderers == null || cac.renderers.Count == 0) {
				cac.renderers = new List<Renderer>();
				cac.renderers.Add(cac.GetComponent<MeshRenderer>());
			}
			foreach (Renderer renderer in cac.renderers) {
				if (renderer.material.color != cac.color) {
					renderer.material.color = cac.color;
				}
			}
		}
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is ColorableComponent) {

		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is ColorableComponent) {
			
		}
	}
}
