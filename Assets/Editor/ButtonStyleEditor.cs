using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XVerse.UI {
    [CustomEditor(typeof(ButtonStyle))]
    public class ButtonStyleEditor : Editor {
        SerializedProperty defs, sprites, imageType;

        private void OnEnable() {
            defs = serializedObject.FindProperty("defaultSprite");
            sprites = serializedObject.FindProperty("sprites");
            imageType = serializedObject.FindProperty("imageType");
        }

        public override void OnInspectorGUI() {

            serializedObject.Update();
            EditorGUILayout.PropertyField(defs);
            EditorGUILayout.PropertyField(sprites);
            EditorGUILayout.PropertyField(imageType);

            GUILayout.Space(10f);
            bool set = GUILayout.Button("Set as Default");
            if (set) EUtils.SetDefault<ButtonStyle>((ButtonStyle)serializedObject.targetObject);
            serializedObject.ApplyModifiedProperties();
        }
    }
}