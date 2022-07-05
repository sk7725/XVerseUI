using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class CompileToScene {
    [MenuItem("Assets/Compile XUI to Scene")]
    public static void Compile() {
        if (Selection.activeObject is MonoScript m) {
            Type t = m.GetClass();
            if (t == null || !typeof(ICompileUI).IsAssignableFrom(t)) return;
            //EUtils.CompileUI(t);
        }
    }

    [MenuItem("Assets/Compile XUI to Scene", true)]
    private static bool CompileCheck() {
        //todo if public static Build(Canvas main) method exists (use reflection)
        if (Selection.activeObject is MonoScript m) {
            Type t = m.GetClass();
            if (t == null) return false;
            return typeof(ICompileUI).IsAssignableFrom(t);
        }
        return false;
    }
}
