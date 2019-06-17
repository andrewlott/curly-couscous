﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButtonTouchComponent : TouchComponent {
	public override void ComponentStart() {
		BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
		if (boxCollider == null) {
			gameObject.AddComponent<BoxCollider>();
		}
	}

	public override void ComponentDestroy() {
		BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
		if (boxCollider != null) {
			GameObject.Destroy(boxCollider);
		}
	}

	public void OnMouseDownAsButton() {
		// Highlight
	}

	public void OnMouseUpAsButton() { 
		// Toggle Selected State
        // HAX
        if (GameController.Instance.IsEnabled) {
            this.gameObject.AddComponent<TouchComponent>();
        }
    }
}
