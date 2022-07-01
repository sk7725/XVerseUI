using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XVerse.UI {
    public class Styles : ScriptableObject {
        public const string DIR = "Settings/Styles", FOLDER = "Settings";
        public static Styles main;
        public StylesMap defaults = new StylesMap();

        public static T Default<T>() where T : ScriptableObject {
            if (main == null) main = Resources.Load<Styles>(DIR);
            if (main == null) {
                Debug.LogError("No Styles ScriptableObject preset found, Please assign at least one default style");
                return null;
            }
            T get = main.defaults.Get<T>();
            if (get == null) {
                Debug.LogError("No default found for style " + typeof(T).Name);
            }
            return get;
        }
    }
}