using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XVerse.UI {
    [CustomEditor(typeof(XButton))]
    public class XButtonEditor : Editor {
        SerializedProperty style, interactable, pressed, hovering, nav, bg;

        private void OnEnable() {
            style = serializedObject.FindProperty("style");
            interactable = serializedObject.FindProperty("m_Interactable");
            pressed = serializedObject.FindProperty("pressed");
            hovering = serializedObject.FindProperty("hovering");
            nav = serializedObject.FindProperty("m_Navigation");
            bg = serializedObject.FindProperty("background");
        }
        public override void OnInspectorGUI() {

            serializedObject.Update();
            EditorGUILayout.PropertyField(style);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(" ");
            EUtils.AddStylePath((ButtonStyle)style.objectReferenceValue, ref style);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(bg);

            EditorGUILayout.PropertyField(interactable);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(pressed);
            EditorGUILayout.PropertyField(hovering);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(nav);

            /*GUILayout.BeginHorizontal(); 
            GUILayout.Label("Interactable", GUILayout.Width(labelWidth)); 
            b.interactable = GUILayout.Toggle(b.interactable, ""); 
            GUILayout.EndHorizontal();*/

            //GUILayout.Space(10f);
            //GUILayout.Button("Set as Default");
            //DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
        }

        
    }
}