using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : BaseComponent {
	public string Trigger;
	public delegate void CallbackFunction(GameObject g);
	public CallbackFunction Callback;
	public string CallbackState;
}
