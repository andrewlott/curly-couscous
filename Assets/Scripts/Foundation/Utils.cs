﻿using System;
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

	public static int RandomInt(int max) {
		return rng.Next(max);
	}

	public static float RandomFloat(float max) {
		return (float)(rng.NextDouble() * max);
	}

	public static void DestroyEntity(GameObject g) {
		BaseComponent[] allComponents = g.GetComponents<BaseComponent>();
		foreach(BaseComponent c in allComponents) {
			Pool.Instance.RemoveComponent(c);
		}

		GameObject.Destroy(g);
	}

}