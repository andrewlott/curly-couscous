using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorableComponent : BaseComponent {
	public List<Renderer> renderers;
	public Color color;

    public void SetColor() {
        if (renderers == null || renderers.Count == 0) {
            renderers = new List<Renderer>();
            renderers.Add(this.gameObject.GetComponent<MeshRenderer>());
        }
        foreach (Renderer r in renderers) {
            if (r.material.color != color) {
                r.material.color = color;
            }
        }
    }

	public void SetBackground() {
		GameController gc = GameController.Instance;
		gc.background.GetComponent<SpriteRenderer>().color = gc.previousColor;
	}
}
