using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EUtils {
    public static T FromPath<T>(string path) where T : ScriptableObject {
        return AssetDatabase.LoadAssetAtPath<T>(path);
    }

    public static void SetDefault<T>(T obj) where T : ScriptableObject {
        Debug.Log("TODO");
    }
}
