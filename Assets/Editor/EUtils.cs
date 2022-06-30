using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XVerse.UI;

public static class EUtils {
    private const string RES_DIR = "Assets/Resources";
    private static char[] folderSplit = { '/', '\\' };

    public static T FromPath<T>(string path) where T : ScriptableObject {
        return AssetDatabase.LoadAssetAtPath<T>(path);
    }

    public static void SetDefault<T>(T obj) where T : ScriptableObject {
        Debug.Log("TODO turn into an addressable");

        if (!AssetDatabase.IsValidFolder("Assets/Resources")) {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        if (!AssetDatabase.IsValidFolder(RES_DIR + "/" + Styles.FOLDER)) {
            AssetDatabase.CreateFolder(RES_DIR, Styles.FOLDER);
        }

        string path = RES_DIR + "/" + Styles.DIR + ".asset";

        Styles defo = AssetDatabase.LoadAssetAtPath<Styles>(path); //should be an addressable fetcher
        if (defo == null) {
            defo = ScriptableObject.CreateInstance<Styles>();
            AssetDatabase.CreateAsset(defo, path);
            //should add defo to addressable here
        }

        defo.defaults.Set<T>(obj);
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
