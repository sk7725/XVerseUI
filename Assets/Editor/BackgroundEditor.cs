using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XVerse.UI {
    [CustomEditor(typeof(Background))]
    public class BackgroundEditor : Editor {
        SerializedProperty style, color, overrideColor, rTarget, rPadding, maskable;

        private void OnEnable() {
            style = serializedObject.FindProperty("style");
            color = serializedObject.FindProperty("m_Color");
            overrideColor = serializedObject.FindProperty("overrideColor");
            rTarget = serializedObject.FindProperty("m_RaycastTarget");
            rPadding = serializedObject.FindProperty("m_RaycastPadding");
            maskable = serializedObject.FindProperty("m_Maskable");
        }
        public override void OnInspectorGUI() {

            serializedObject.Update();
            EditorGUILayout.PropertyField(style);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(" ");
            EUtils.AddStylePath((BackgroundStyle)style.objectReferenceValue, ref style);
            EditorGUILayout.EndHorizontal();

            Color prevColor = color.colorValue;
            EditorGUILayout.PropertyField(color); //when changed, set overrideColor to true
            if (color.colorValue != prevColor) {
                Debug.Log("Color changed!");
                overrideColor.boolValue = true;
            }

            EditorGUILayout.BeginHorizontal();
            BackgroundStyle bs = (BackgroundStyle)style.objectReferenceValue;
            EditorGUI.BeginDisabledGroup(bs == null || !overrideColor.boolValue);
            EditorGUILayout.PrefixLabel(" ");
            bool set = GUILayout.Button("Reset");
            if (set) {
                color.colorValue = bs.color;
                overrideColor.boolValue = false;
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(rTarget);
            EditorGUILayout.PropertyField(rPadding);
            EditorGUILayout.PropertyField(maskable);

            serializedObject.ApplyModifiedProperties();
        }


    }
}