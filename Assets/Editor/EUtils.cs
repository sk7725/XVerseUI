using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EUtils {
    private const string RES_DIR = "Assets/Resources";
    private const string DEFAULTS_DIR = "Assets/Resources/DefaultValues";
    private static char[] folderSplit = { '/', '\\' };

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
            defo.name = "Default" + typeof(T).Name;
            AssetDatabase.CreateAsset(defo, path);
        }

        EditorUtility.CopySerialized(obj, defo);
        EditorUtility.SetDirty(defo);
        Debug.Log("Set default of " + typeof(T).Name + " to " + obj.name);
    }

    public static void AddStylePath<T>(T current, ref SerializedProperty prop) where T : ScriptableObject {
        string[] stylesPath = GetAllPaths(typeof(T).Name);
        string[] styles = new string[stylesPath.Length + 1];
        styles[0] = "None";
        int c = 0;

        for (int i = 0; i < stylesPath.Length; i++) {
            string path = stylesPath[i];
            int f = path.LastIndexOfAny(folderSplit) + 1;
            int l = path.IndexOf(".");
            styles[i + 1] = path.Substring(f, l - f);
            if (current != null && current.name == styles[i + 1]) c = i + 1;
        }
        int style = EditorGUILayout.Popup(c, styles);

        if (style < 1) prop.objectReferenceValue = null;
        else {
            prop.objectReferenceValue = EUtils.FromPath<T>(stylesPath[style - 1]);
        }
    }

    private static string[] GetAllPaths(string name) {
        string[] guids = AssetDatabase.FindAssets("t:" + name);
        for (int i = 0; i < guids.Length; i++) {
            guids[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
        }

        return guids;
    }
}
