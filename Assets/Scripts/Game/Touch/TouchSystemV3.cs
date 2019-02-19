using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchSystem : BaseSystem {
	public override void Start() {
	}

	public override void Stop() {
	}

	public override void Update() {
		CheckTouch();
	}

	public override void OnComponentAdded(BaseComponent c) {
	}

	public override void OnComponentRemoved(BaseComponent c) {
	}

	private GameObject GetTouchedObject() {
		Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
		RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
		if (hitInformation.collider != null) {
			GameObject touchedObject = hitInformation.transform.gameObject;
			return touchedObject;
		}

		return null;
	}

	private void CheckTouch() {
		if (Input.GetMouseButtonDown(0)) {
			GameObject touchedObject = GetTouchedObject();
			if (touchedObject != null) {
				TouchComponent tc = touchedObject.GetComponent<TouchComponent>();
				if (tc != null) {
					OnTouchDownForComponent(tc);
				}
			}
		} else if (Input.GetMouseButtonUp(0)) {
			GameObject touchedObject = GetTouchedObject();
			if (touchedObject != null) {
				TouchComponent tc = touchedObject.GetComponent<TouchComponent>();
				if (tc != null) {
					OnTouchUpForComponent(tc);
				}
			}
		}
	}

	private void OnTouchDownForComponent(BaseComponent c) {
		if (c is ColorButtonTouchComponent) {
			Debug.Log(c);
		}
	}
		
	private void OnTouchUpForComponent(BaseComponent c) {
		if (c is ColorButtonTouchComponent) {
			Debug.Log(c);
		}
	}
}