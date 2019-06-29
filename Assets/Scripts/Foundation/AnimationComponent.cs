using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : BaseComponent {
	public string Trigger;
    public bool LeaveTriggerOn;
	public delegate void CallbackFunction(GameObject g);
	public CallbackFunction Callback;
	public string CallbackState;

    public static void Animate(GameObject g, string trigger, bool leaveTriggerOn, AnimationComponent.CallbackFunction callback, string callbackState) {
        AnimationComponent ac = g.AddComponent<AnimationComponent>();
        ac.Trigger = trigger;
        ac.Callback = callback;
        ac.LeaveTriggerOn = leaveTriggerOn;
        ac.CallbackState = callbackState;
    }
}
