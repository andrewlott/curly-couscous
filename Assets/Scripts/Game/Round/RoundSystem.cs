using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : BaseSystem {
	private static int Round = 0;

	public override void Start() {
		Pool.Instance.AddSystemListener(typeof(MatchComponent), this);
	}

	public override void Stop() {
		Pool.Instance.RemoveSystemListener(typeof(MatchComponent), this);
	}

	public override void Update() {
		if (Round == 0) {
			Round++;
			Reset(Round);
		}

		// loop through all matchables, get their colorables, if all same color make matched component
	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is MatchComponent) {
			Round++;
			Reset(Round);
			GameObject.Destroy(c);
		}
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is ColorComponent) {
			
		}
	}

	private void Reset(int round) {
		// get all the buttons and 2 boxes and background
		// set background to old box color
		// choose next color for box, reset color of player box
		// choose other colors for buttons, set their colorables to that and change colorable to handle color change
	}
}
