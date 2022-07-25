using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace XVerse.UI {
    [CustomEditor(typeof(LabelStyle))]
    public class LabelStyleEditor : Editor {
        SerializedProperty font, mat, overflow, wrap, defaultSize, defaultColor;
        bool showDefaults = true;
        Material[] m_MaterialPresets;
        GUIContent[] m_MaterialPresetNames;
        int m_MaterialPresetSelectionIndex;
        protected Dictionary<int, int> m_MaterialPresetIndexLookup = new Dictionary<int, int>();
        static readonly GUIContent k_FontAssetLabel = new GUIContent("Font Asset", "The Font Asset containing the glyphs that can be rendered for this text. Value overriden by style.");
        static readonly GUIContent k_MaterialPresetLabel = new GUIContent("Material Preset", "The material used for rendering. Only materials created from the Font Asset can be used.");
        static readonly GUIContent k_WrappingLabel = new GUIContent("Wrapping", "Wraps text to the next line when reaching the edge of the container.");
        static readonly GUIContent k_OverflowLabel = new GUIContent("Overflow", "How to display text which goes past the edge of the container.");
        
        static readonly GUIContent k_DefaultsLabel = new GUIContent("Defaults", "Default settings are only applied when an instance of a component is first created.");

        private void OnEnable() {
            font = serializedObject.FindProperty("font");
            mat = serializedObject.FindProperty("material");
            overflow = serializedObject.FindProperty("overflow");
            wrap = serializedObject.FindProperty("wrap");
            defaultSize = serializedObject.FindProperty("defaultTextFont");
            defaultColor = serializedObject.FindProperty("defaultTextColor");

            m_MaterialPresetNames = GetMaterialPresets();
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            DrawFont();
            EditorGUILayout.PropertyField(overflow, k_OverflowLabel);
            EditorGUILayout.PropertyField(wrap, k_WrappingLabel);

            GUILayout.Space(10f);
            showDefaults = EditorGUILayout.BeginFoldoutHeaderGroup(showDefaults, k_DefaultsLabel);
            if (showDefaults) {
                EditorGUILayout.PropertyField(defaultSize);
                EditorGUILayout.PropertyField(defaultColor);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            GUILayout.Space(10f);
            bool save = GUILayout.Button("Apply Modifications");
            if (save) EUtils.SaveStyle<Label, LabelStyle>((LabelStyle)serializedObject.targetObject);
            bool set = GUILayout.Button("Set as Default");
            if (set) EUtils.SetDefault((LabelStyle)serializedObject.targetObject);
            serializedObject.ApplyModifiedProperties();
        }

        void DrawFont() {
            bool isFontAssetDirty = false;

            // FONT ASSET
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(font, k_FontAssetLabel);
            if (EditorGUI.EndChangeCheck()) {
                //m_HavePropertiesChanged = true;
                //m_HasFontAssetChangedProp.boolValue = true;

                // Get new Material Presets for the new font asset
                m_MaterialPresetNames = GetMaterialPresets();
                m_MaterialPresetSelectionIndex = 0;

                isFontAssetDirty = true;
            }

            Rect rect;

            // MATERIAL PRESET
            if (m_MaterialPresetNames != null && !isFontAssetDirty) {
                EditorGUI.BeginChangeCheck();
                rect = EditorGUILayout.GetControlRect(false, 17);

                EditorGUI.BeginProperty(rect, k_MaterialPresetLabel, mat);

                float oldHeight = EditorStyles.popup.fixedHeight;
                EditorStyles.popup.fixedHeight = rect.height;

                int oldSize = EditorStyles.popup.fontSize;
                EditorStyles.popup.fontSize = 11;

                if (mat.objectReferenceValue != null)
                    m_MaterialPresetIndexLookup.TryGetValue(mat.objectReferenceValue.GetInstanceID(), out m_MaterialPresetSelectionIndex);

                m_MaterialPresetSelectionIndex = EditorGUI.Popup(rect, k_MaterialPresetLabel, m_MaterialPresetSelectionIndex, m_MaterialPresetNames);

                EditorGUI.EndProperty();
                if (EditorGUI.EndChangeCheck()) {
                    mat.objectReferenceValue = m_MaterialPresets[m_MaterialPresetSelectionIndex];
                    //m_HavePropertiesChanged = true;
                }

                EditorStyles.popup.fixedHeight = oldHeight;
                EditorStyles.popup.fontSize = oldSize;
            }
        }

        protected GUIContent[] GetMaterialPresets() {
            TMP_FontAsset fontAsset = font.objectReferenceValue as TMP_FontAsset;
            if (fontAsset == null) return null;

            m_MaterialPresets = TMP_EditorUtility.FindMaterialReferences(fontAsset);
            m_MaterialPresetNames = new GUIContent[m_MaterialPresets.Length];

            m_MaterialPresetIndexLookup.Clear();

            for (int i = 0; i < m_MaterialPresetNames.Length; i++) {
                m_MaterialPresetNames[i] = new GUIContent(m_MaterialPresets[i].name);

                m_MaterialPresetIndexLookup.Add(m_MaterialPresets[i].GetInstanceID(), i);

                //if (m_TargetMaterial.GetInstanceID() == m_MaterialPresets[i].GetInstanceID())
                //    m_MaterialPresetSelectionIndex = i;
            }

            //m_IsPresetListDirty = false;

            return m_MaterialPresetNames;
        }
    }
}