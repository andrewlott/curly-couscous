/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchSystem : BaseSystem {
	private TileTouchComponent _previousTouchComponent;
	private TileTouchComponent _lastTouchComponent;

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
				SwappingComponent sc = touchedObject.GetComponent<SwappingComponent>();
				if (tc != null && sc == null) {
					OnTouchDownForComponent(tc);
				}
			}
		} else if (Input.GetMouseButtonUp(0)) {
			GameObject touchedObject = GetTouchedObject();
			if (touchedObject != null) {
				TouchComponent tc = touchedObject.GetComponent<TouchComponent>();
				SwappingComponent sc = touchedObject.GetComponent<SwappingComponent>();
				if (tc != null && sc == null) {
					OnTouchUpForComponent(tc);
				}
			}
		}
	}

	private void OnTouchDownForComponent(BaseComponent c) {
		if (c is TileTouchComponent) {
			TileComponent tc = c.gameObject.GetComponent<TileComponent>();
			GridComponent gc = tc.ParentGrid;
			if (gc.IsHidden(c.gameObject)) {
				return;
			}

			_lastTouchComponent = c as TileTouchComponent;
			AddCursor(_lastTouchComponent);

			Vector2Int pos = gc.PositionInGrid(c.gameObject);
			Debug.Log(pos);
		}
	}
		
	private void OnTouchUpForComponent(BaseComponent c) {
		if (c is TileTouchComponent) {
			TileComponent tc = c.gameObject.GetComponent<TileComponent>();
			GridComponent gc = tc.ParentGrid;

			if (gc.IsHidden(c.gameObject)) {
				return;
			}

			// Touch up on the same tile that you touched down on (first time)
			// select and queue to previous
			// Touch up on the same tile that you touched down on (second time) 
			// deselect and clear previous and current

			// Touch down on a tile and touch up somewhere else
			if (_previousTouchComponent == c) {
				RemoveCursor(_previousTouchComponent);
				_previousTouchComponent = null;
				_lastTouchComponent = null;
			} else if (_lastTouchComponent == c) {
				if (_previousTouchComponent == null) {
					_previousTouchComponent = _lastTouchComponent;
					_lastTouchComponent = null;
				} else {
					if (gc.AreNeighbors(_previousTouchComponent.gameObject, _lastTouchComponent.gameObject)) {
						Swap();
					} else {
						RemoveCursor(_previousTouchComponent);
						RemoveCursor(_lastTouchComponent);
						_previousTouchComponent = null;
						_lastTouchComponent = null;
					}
				}
			} else if (_previousTouchComponent == null) {
				_previousTouchComponent = _lastTouchComponent;
				_lastTouchComponent = gc.NearestNeighbor(_previousTouchComponent.gameObject, c.gameObject).GetComponent<TileTouchComponent>();

				AddCursor(_lastTouchComponent);
				Swap();

			} else {
				_previousTouchComponent = null;
				_lastTouchComponent = null;
			}
		}
	}

	private void Swap() {
		SwapComponent swapComponent = _previousTouchComponent.gameObject.AddComponent<SwapComponent>();
		swapComponent.Objects.Add(swapComponent.gameObject);
		swapComponent.Objects.Add(_lastTouchComponent.gameObject);

		_previousTouchComponent = null;
		_lastTouchComponent = null;
	}

	private void AddCursor(TileTouchComponent ttc) {
		TileSelectedComponent tsc = ttc.gameObject.GetComponent<TileSelectedComponent>();
		if (tsc == null) {
			tsc = ttc.gameObject.AddComponent<TileSelectedComponent>();
		}
	}

	private void RemoveCursor(TileTouchComponent ttc) {
		TileSelectedComponent tsc = ttc.gameObject.GetComponent<TileSelectedComponent>();
		if (tsc != null) {
			GameObject.Destroy(tsc);
		}
	}
}
*/