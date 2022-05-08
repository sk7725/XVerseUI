using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EUtils {
    private const string RES_DIR = "Assets/Resources";
    private const string DEFAULTS_DIR = "Assets/Resources/DefaultValues";

    public static T FromPath<T>(string path) where T : ScriptableObject {
        return AssetDatabase.LoadAssetAtPath<T>(path);
    }

    public static void SetDefault<T>(T obj) where T : ScriptableObject {
        Debug.Log("TODO");

        if (!AssetDatabase.IsValidFolder(RES_DIR)) {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        if (!AssetDatabase.IsValidFolder(DEFAULTS_DIR)) {
            AssetDatabase.CreateFolder("Assets/Resources", "DefaultValues");
        }

        string path = DEFAULTS_DIR + "/Default" + typeof(T).Name + ".asset";

        T defo = AssetDatabase.LoadAssetAtPath<T>(path);
        if (defo == null) {
            defo = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(defo, path);
        }

        EditorUtility.CopySerialized(obj, defo);
        EditorUtility.SetDirty(defo);
        Debug.Log("Set default of " + typeof(T).Name + " to " + obj.name);
    }
}
