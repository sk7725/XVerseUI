using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Styles {
    private const string DEFAULTS_DIR = "DefaultValues";
    private static DefaultMap defaults = new DefaultMap();

    public static T Default<T>() where T: ScriptableObject {
        T get = defaults.Get<T>();
        if(get == null) {
            Debug.Log("Load new default from path: " + DEFAULTS_DIR + "/Default" + typeof(T).Name);
            get = Resources.Load<T>(DEFAULTS_DIR + "/Default" + typeof(T).Name);
            if(get != null) defaults.Set(get);
        }
        return get;
    }
}
