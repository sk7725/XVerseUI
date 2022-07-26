using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XVerse.UI {
    [CustomEditor(typeof(ButtonStyle))]
    public class ButtonStyleEditor : Editor {
        bool showDefaults = true;
        SerializedProperty defs, sprites, imageType, defaultSize, defaultColor;
        static readonly GUIContent k_DefaultsLabel = new GUIContent("Defaults", "Default settings are only applied when an instance of a component is first created.");

        private void OnEnable() {
            defs = serializedObject.FindProperty("defaultSprite");
            sprites = serializedObject.FindProperty("sprites");
            imageType = serializedObject.FindProperty("imageType");
            //defaultLabelStyle = serializedObject.FindProperty("defaultLabelStyle");
            defaultSize = serializedObject.FindProperty("defaultTextFont");
            defaultColor = serializedObject.FindProperty("defaultTextColor");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(defs);
            EditorGUILayout.PropertyField(sprites);
            EditorGUILayout.PropertyField(imageType);

            GUILayout.Space(10f);
            showDefaults = EditorGUILayout.BeginFoldoutHeaderGroup(showDefaults, k_DefaultsLabel);
            if (showDefaults) {
                /*
                EditorGUILayout.PropertyField(defaultLabelStyle);
                if (defaultLabelStyle.objectReferenceValue != null) {
                    EditorGUI.BeginDisabledGroup(true);
                    LabelStyle s = (LabelStyle)defaultLabelStyle.objectReferenceValue;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Default Text Font");
                    EditorGUILayout.FloatField(s.defaultTextFont);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Default Text Color");
                    EditorGUILayout.ColorField(s.defaultTextColor);
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.EndDisabledGroup();
                }*/
                EditorGUILayout.PropertyField(defaultSize);
                EditorGUILayout.PropertyField(defaultColor);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            GUILayout.Space(10f);
            bool save = GUILayout.Button("Apply Modifications");
            if (save) EUtils.SaveStyle<XButton, ButtonStyle>((ButtonStyle)serializedObject.targetObject);
            bool set = GUILayout.Button("Set as Default");
            if (set) EUtils.SetDefault((ButtonStyle)serializedObject.targetObject);
            serializedObject.ApplyModifiedProperties();
        }
    }
}