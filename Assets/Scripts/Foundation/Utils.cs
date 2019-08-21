using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
	private static System.Random rng = new System.Random();  

	public static void Shuffle<T>(this IList<T> list) {  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = RandomInt(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

    public static bool RandomBool() {
        return Utils.RandomInt(2) > 0 ? true : false;
    }

	public static int RandomInt(int max) {
		return rng.Next(max);
	}

	public static float RandomFloat(float max) {
		return (float)(rng.NextDouble() * max);
	}

    public static Color RandomColor() {
        return new Color(Utils.RandomFloat(1.0f), Utils.RandomFloat(1.0f), Utils.RandomFloat(1.0f));
    }

    public static void DestroyEntity(GameObject g) {
		BaseComponent[] allComponents = g.GetComponents<BaseComponent>();
		foreach(BaseComponent c in allComponents) {
			Pool.Instance.RemoveComponent(c);
		}

		GameObject.Destroy(g);
	}

    public static void TriggerAnimation(GameObject g, string trigger) {
        GameController.Instance.HandleWaitAndDo(Utils.RandomFloat(0.0f), () => {
            g.GetComponent<Animator>().SetBool(trigger, true);
        });
    }
}
