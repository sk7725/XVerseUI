using System;
using System.Collections;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;

[Serializable]
public class StylesMap {  
    [SerializeField]
    public List<ClassTypeReference> key = new List<ClassTypeReference>();
    [SerializeField]
    public List<ScriptableObject> value = new List<ScriptableObject>();

    public void Set<T>(T v) where T:ScriptableObject {
        if (key.Contains(typeof(T))) {
            int id = key.IndexOf(typeof(T));
            value[id] = v;
        }
        else {
            key.Add(typeof(T));
            value.Add(v);
        }
    }

    public T Get<T>() where T:ScriptableObject {
        int id = key.IndexOf(typeof(T));
        if (id >= 0) return (T)value[id];
        return default(T);
    }

    public bool Contains<T>() {
        return key.Contains(typeof(T));
    }
}
