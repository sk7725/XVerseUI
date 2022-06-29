using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XVerse.UI {
    [CustomEditor(typeof(BackgroundStyle))]
    public class BackgroundStyleEditor : Editor {
        SerializedProperty defs, color, mat, imageType;

        private void OnEnable() {
            defs = serializedObject.FindProperty("sprite");
            color = serializedObject.FindProperty("color");
            mat = serializedObject.FindProperty("material");
            imageType = serializedObject.FindProperty("imageType");
        }

        public override void OnInspectorGUI() {

            serializedObject.Update();
            EditorGUILayout.PropertyField(defs);
            EditorGUILayout.PropertyField(color);
            EditorGUILayout.PropertyField(mat);
            EditorGUILayout.PropertyField(imageType);

            GUILayout.Space(10f);
            bool set = GUILayout.Button("Set as Default");
            if (set) EUtils.SetDefault((BackgroundStyle)serializedObject.targetObject);
            serializedObject.ApplyModifiedProperties();
        }
    }
}