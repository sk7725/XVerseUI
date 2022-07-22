using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using XVerse.UI;
using UnityEditor.SceneManagement;

public class CompileToScene {
    [MenuItem("Assets/Compile XUI to Scene")]
    public static void Compile() {
        if (Selection.activeObject is MonoScript m) {
            MethodInfo method = GetBuildMethod(m);
            if(method != null) {
                //find game object
                GameObject go = GameObject.Find("_"+m.name);
                if (go == null) {
                    go = new GameObject("_" + m.name);
                    Canvas mainc = GameObject.FindObjectOfType<Canvas>();
                    if (mainc == null) {
                        Debug.LogError("Cannot find a canvas to attach the UI to!");
                        return;
                    }
                    go.transform.SetParent(mainc.gameObject.transform, false);
                }
                else {
                    Transform parent = go.transform.parent; //preserve hierarchy
                    int index = go.transform.GetSiblingIndex();
                    UnityEngine.Object.DestroyImmediate(go);
                    go = new GameObject("_" + m.name);
                    go.transform.SetParent(parent, false);
                    go.transform.SetSiblingIndex(index);
                }

                //add canvas
                Canvas canvas = go.AddComponent<Canvas>();
                FillRect(go.GetComponent<RectTransform>());

                //add the compiler
                Component c = go.AddComponent(m.GetClass());
                method.Invoke(c, new object[] { canvas });
                UnityEngine.Object.DestroyImmediate(canvas);

                //remove the compiler
                GameObject.DestroyImmediate(c);
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            }
        }
    }

    [MenuItem("Assets/Compile XUI to Scene", true)]
    private static bool CompileCheck() {
        //todo if public static Build(Canvas main) method exists (use reflection)
        if (Selection.activeObject is MonoScript m) {
            return GetBuildMethod(m) != null;
        }
        return false;
    }

    private static MethodInfo GetBuildMethod(MonoScript m) {
        Type t = m.GetClass();
        if (t == null || !(typeof(MonoBehaviour).IsAssignableFrom(t))) {
            return null;
        }

        /*foreach (MethodInfo mm in t.GetMethods()) {
            Debug.Log(mm.Name);
        }*/

        MethodInfo method = t.GetMethod("Build");
        if (method == null) return null;
        ParameterInfo[] parameters = method.GetParameters();

        /*for (int i = 0; i < parameters.Length; i++) {
            Debug.Log(parameters[i].ParameterType);
        }*/

        if(parameters.Length == 1 && parameters[0].ParameterType == typeof(Canvas)) return method;
        return null;
    }

    private static void FillRect(RectTransform r) {
        r.pivot = new Vector2(0.5f, 0.5f);
        r.offsetMin = new Vector2(0f, 0f);
        r.offsetMax = new Vector2(0f, 0f);
        r.anchorMin = new Vector2(0f, 0f);
        r.anchorMax = new Vector2(1f, 1f);
    }
}
