using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DefaultMap {
    [SerializeField]
    private List<Type> key = new List<Type>();
    [SerializeField]
    private List<object> value = new List<object>();

    public void Set<T>(T v) {
        if (key.Contains(typeof(T))) {
            int id = key.IndexOf(typeof(T));
            value[id] = v;
        }
        else {
            key.Add(typeof(T));
            value.Add(v);
        }
    }

    public T Get<T>() {
        int id = key.IndexOf(typeof(T));
        if (id >= 0) return (T)value[id];
        return default(T);
    }

    public bool Contains<T>() {
        return key.Contains(typeof(T));
    }
}
