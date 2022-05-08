using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Styles {
    private const string DEFAULTS_DIR = "Assets/Resources/DefaultValues";
    private static DefaultMap defaults = new DefaultMap();

    public static T Default<T>() where T: ScriptableObject {
        T get = defaults.Get<T>();
        if(get == null) {
            get = Resources.Load<T>(DEFAULTS_DIR + "/Default" + typeof(T).Name + ".asset");
            if(get != null) defaults.Set(get);
        }
        return get;
    }
}
