using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XTown.UI {
    [CustomEditor(typeof(XButton))]
    public class XButtonEditor : Editor {
        private static char[] folderSplit = { '/', '\\' };
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
            AddStylePath((ButtonStyle)style.objectReferenceValue);
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

        private void AddStylePath(ButtonStyle current) {
            string[] stylesPath = GetAllPaths("ButtonStyle");
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

            if (style < 1) this.style.objectReferenceValue = null;
            else {
                this.style.objectReferenceValue = EUtils.FromPath<ButtonStyle>(stylesPath[style - 1]);
            }
        }

        private string[] GetAllPaths(string name) {
            string[] guids = AssetDatabase.FindAssets("t:" + name);
            for (int i = 0; i < guids.Length; i++) {
                guids[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            }

            return guids;
        }
    }
}