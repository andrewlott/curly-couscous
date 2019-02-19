/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: If last touch is on a tile that's not touchable anymore, remove it

public class TouchSystem : BaseSystem {
	private TileSelectedComponent _firstSelectedTile;
	private TileSelectedComponent _secondSelectedTile;

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
	// on TD queue sel tile, if TD on another one then sel both (prob in separate variables)
	// if touch up on first one then only select that one, wait for TD and TU on another, if it's lr neighbor then swap otherwise select that one
	// if td and tu on same one that's selected then unselect
	// if TU on neighbor of tile to the L or R of tile, then swap with neighbor (this is drag)

	// if you want to show cursor while swapping then needs to be separated from prefab

	private void OnTouchDownForComponent(BaseComponent c) {
		if (c is TileTouchComponent) {
			if (GC.IsHidden(c.gameObject)) {
				return;
			}
				
			if (_firstSelectedTile == null) {
				_firstSelectedTile = c.gameObject.AddComponent<TileSelectedComponent>();
			} else if (_firstSelectedTile.gameObject != c.gameObject) {
				if (GC.PositionInGrid(c.gameObject).y > GC.PositionInGrid(_firstSelectedTile.gameObject).y &&
					GC.AreNeighbors(c.gameObject, GC.RightNeighbor(c.gameObject))) {
					_secondSelectedTile = GC.RightNeighbor(c.gameObject).AddComponent<TileSelectedComponent>();
				} else if (GC.PositionInGrid(c.gameObject).y < GC.PositionInGrid(_firstSelectedTile.gameObject).y &&
					GC.AreNeighbors(c.gameObject, GC.LeftNeighbor(c.gameObject))) {
					_secondSelectedTile = GC.LeftNeighbor(c.gameObject).AddComponent<TileSelectedComponent>();
				} else {
					_secondSelectedTile = _firstSelectedTile;
				}
			}

			Vector2Int pos = GC.PositionInGrid(c.gameObject);
			Debug.Log(pos);
		}
	}
		
	private void OnTouchUpForComponent(BaseComponent c) {
		if (c is TileTouchComponent) {
			if (GC.IsHidden(c.gameObject)) {
				return;
			}

			if (c.gameObject == _firstSelectedTile.gameObject) {
				if (c.gameObject == _secondSelectedTile.gameObject) {
					GameObject.Destroy(_firstSelectedTile);
					GameObject.Destroy(_secondSelectedTile);
					_firstSelectedTile = null;
					_secondSelectedTile = null;
				}
				// Otherwise Pass
			} else if (_secondSelectedTile != null && GC.AreNeighbors(c.gameObject, _secondSelectedTile.gameObject)) {
				SwapComponent swapComponent = _firstSelectedTile.gameObject.AddComponent<SwapComponent>();
				swapComponent.Objects.Add(swapComponent.gameObject);
				swapComponent.Objects.Add(c.gameObject);

				GameObject.Destroy(_firstSelectedTile.GetComponent<TileSelectedComponent>());
				GameObject.Destroy(_secondSelectedTile.GetComponent<TileSelectedComponent>());
			}
		}
	}
}
*/