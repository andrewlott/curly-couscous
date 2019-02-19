using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Pool {
	private Dictionary<Type, List<BaseComponent>> ComponentsDictionary = new Dictionary<Type, List<BaseComponent>>();
	private Dictionary<Type, List<BaseSystem>> SystemsDictionary = new Dictionary<Type, List<BaseSystem>>();

	private static Pool _instance;

	public static Pool Instance { 
		get { 
			if (_instance == null) {
				_instance = new Pool();
			} 
			return _instance; 
		} 
	}

	public List<BaseComponent> ComponentsLike(BaseComponent c) {
		return ComponentsForType(c.GetType());
	}

	public BaseComponent ComponentForType(Type type) {
		List<BaseComponent> components = this.ComponentsForType(type);
		if (components.Count == 0) {
			return null;
		}
		return components.First();
	}

	public List<BaseComponent> ComponentsForType(Type type) {
		if (!ComponentsDictionary.ContainsKey(type)) {
			ComponentsDictionary.Add(type, new List<BaseComponent>());
		}

		return ComponentsDictionary[type];
	}

	public List<BaseSystem> SystemsForComponentLike(BaseComponent c) {
	    return SystemsForType(c.GetType());
    }

    public List<BaseSystem> SystemsForType(Type type) {
	    if (!SystemsDictionary.ContainsKey(type)) {
			SystemsDictionary.Add(type, new List<BaseSystem>());
		} 

		return SystemsDictionary[type];
    }

	public void AddSystemListener(Type type, BaseSystem s) {
		List<BaseSystem> systems = SystemsForType(type);
		if (s != null && !systems.Contains(s)) {
			systems.Add(s);
		}
	}

	public void RemoveSystemListener(Type type, BaseSystem s) {
		List<BaseSystem> systems = SystemsForType(type);
		if (s != null && systems.Contains(s)) {
			systems.Remove(s);
		}
	}
		
	public void AddComponent(BaseComponent c) {
		if (c != null) {
			List<BaseComponent> components = ComponentsLike(c);
			components.Add(c);

			List<BaseSystem> systems = SystemsForComponentLike(c);
			foreach (BaseSystem s in systems) {
				s.OnComponentAdded(c);
			}
		}
	}

	public void RemoveComponent(BaseComponent c) {
		if (c != null) {
			List<BaseComponent> components = ComponentsLike(c);
			components.Remove(c);

			List<BaseSystem> systems = SystemsForComponentLike(c);
			foreach (BaseSystem s in systems) {
				s.OnComponentRemoved(c);
			}
		}
	}
}
