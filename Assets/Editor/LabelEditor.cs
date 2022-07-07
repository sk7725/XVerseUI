using System;
using System.Reflection;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace XVerse.UI {
    [CustomEditor(typeof(Label))]
    public class LabelEditor : TMP_EditorPanelUI {
        protected SerializedProperty style, overrideOverflow, overrideWrap;
        object[] voidObject = { };

        protected override void OnEnable() {
            base.OnEnable();
            style = serializedObject.FindProperty("style");
            overrideOverflow = serializedObject.FindProperty("overrideOverflow");
            overrideWrap = serializedObject.FindProperty("overrideWrap");
        }

        public override void OnInspectorGUI() {
            if (IsMixSelectionTypes()) return;
            serializedObject.Update();

            //style
            EditorGUILayout.PropertyField(style);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(" ");
            EUtils.AddStylePath((LabelStyle)style.objectReferenceValue, ref style);
            EditorGUILayout.EndHorizontal();

            //text
            DrawTextInput();
            DrawNewMainSettings();

            DrawExtraSettings();

            EditorGUILayout.Space();

            if (serializedObject.ApplyModifiedProperties() || m_HavePropertiesChanged) {
                m_TextComponent.havePropertiesChanged = true;
                m_HavePropertiesChanged = false;
            }
        }

        protected void DrawNewMainSettings() {
            // MAIN SETTINGS SECTION
            GUILayout.Label(new GUIContent("<b>Main Settings</b>"), TMP_UIStyleManager.sectionHeader);

            DrawFont();

            DrawColor();

            DrawSpacing();

            DrawAlignment();

            DrawWrappingOverflow();

            DrawTextureMapping();
        }

        void DrawWrappingOverflow() {
            LabelStyle bs = (LabelStyle)style.objectReferenceValue;
            Rect rect;
            float w;

            // WRAPPING
            bool prevw = m_EnableWordWrappingProp.boolValue;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(k_WrappingLabel);
            rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
            w = rect.width;
            rect.width = w * 0.75f;
            EditorGUI.PropertyField(rect, m_EnableWordWrappingProp, GUIContent.none);
            if (m_EnableWordWrappingProp.boolValue != prevw) {
                overrideWrap.boolValue = true;
            }
            
            EditorGUI.BeginDisabledGroup(bs == null || !overrideWrap.boolValue);
            rect.x += rect.width;
            rect.width = w * 0.25f;
            bool set = GUI.Button(rect, "Reset");
            if (set) {
                m_EnableWordWrappingProp.boolValue = bs.wrap;
                overrideWrap.boolValue = false;
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            // OVERFLOW
            int prevo = m_TextOverflowModeProp.enumValueIndex;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(k_OverflowLabel);
            rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
            w = rect.width;
            rect.width = w * 0.75f;
            EditorGUI.PropertyField(rect, m_TextOverflowModeProp, GUIContent.none);
            if (m_TextOverflowModeProp.enumValueIndex != prevo) {
                overrideOverflow.boolValue = true;
            }
            
            EditorGUI.BeginDisabledGroup(bs == null || !overrideOverflow.boolValue);
            rect.x += rect.width;
            rect.width = w * 0.25f;
            bool set1 = GUI.Button(rect, "Reset");
            if (set1) {
                m_TextOverflowModeProp.enumValueIndex = (int)bs.overflow;
                overrideOverflow.boolValue = false;
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        static readonly GUIContent k_FontAssetLabel = new GUIContent("Font Asset", "The Font Asset containing the glyphs that can be rendered for this text. Value overriden by style.");
        static readonly GUIContent k_MaterialPresetLabel = new GUIContent("Material Preset", "The material used for rendering. Only materials created from the Font Asset can be used.");
        static readonly GUIContent k_AutoSizeLabel = new GUIContent("Auto Size", "Auto sizes the text to fit the available space.");
        static readonly GUIContent k_FontSizeLabel = new GUIContent("Font Size", "The size the text will be rendered at in points.");
        static readonly GUIContent k_AutoSizeOptionsLabel = new GUIContent("Auto Size Options");
        static readonly GUIContent k_MinLabel = new GUIContent("Min", "The minimum font size.");
        static readonly GUIContent k_MaxLabel = new GUIContent("Max", "The maximum font size.");
        static readonly GUIContent k_WdLabel = new GUIContent("WD%", "Compresses character width up to this value before reducing font size.");
        static readonly GUIContent k_LineLabel = new GUIContent("Line", "Negative value only. Compresses line height down to this value before reducing font size.");
        static readonly GUIContent k_FontStyleLabel = new GUIContent("Font Style", "Styles to apply to the text such as Bold or Italic.");

        static readonly GUIContent k_BoldLabel = new GUIContent("B", "Bold");
        static readonly GUIContent k_ItalicLabel = new GUIContent("I", "Italic");
        static readonly GUIContent k_UnderlineLabel = new GUIContent("U", "Underline");
        static readonly GUIContent k_StrikethroughLabel = new GUIContent("S", "Strikethrough");
        static readonly GUIContent k_LowercaseLabel = new GUIContent("ab", "Lowercase");
        static readonly GUIContent k_UppercaseLabel = new GUIContent("AB", "Uppercase");
        static readonly GUIContent k_SmallcapsLabel = new GUIContent("SC", "Smallcaps");

        static readonly GUIContent k_ColorModeLabel = new GUIContent("Color Mode", "The type of gradient to use.");
        static readonly GUIContent k_BaseColorLabel = new GUIContent("Vertex Color", "The base color of the text vertices.");
        static readonly GUIContent k_ColorPresetLabel = new GUIContent("Color Preset", "A Color Preset which override the local color settings.");
        static readonly GUIContent k_ColorGradientLabel = new GUIContent("Color Gradient", "The gradient color applied over the Vertex Color. Can be locally set or driven by a Gradient Asset.");
        static readonly GUIContent k_CorenerColorsLabel = new GUIContent("Colors", "The color composition of the gradient.");
        static readonly GUIContent k_OverrideTagsLabel = new GUIContent("Override Tags", "Whether the color settings override the <color> tag.");

        static readonly GUIContent k_SpacingOptionsLabel = new GUIContent("Spacing Options (em)", "Spacing adjustments between different elements of the text. Values are in font units where a value of 1 equals 1/100em.");
        static readonly GUIContent k_CharacterSpacingLabel = new GUIContent("Character");
        static readonly GUIContent k_WordSpacingLabel = new GUIContent("Word");
        static readonly GUIContent k_LineSpacingLabel = new GUIContent("Line");
        static readonly GUIContent k_ParagraphSpacingLabel = new GUIContent("Paragraph");

        static readonly GUIContent k_AlignmentLabel = new GUIContent("Alignment", "Horizontal and vertical alignment of the text within its container.");
        static readonly GUIContent k_WrapMixLabel = new GUIContent("Wrap Mix (W <-> C)", "How much to favor words versus characters when distributing the text.");
        static readonly GUIContent k_WrappingLabel = new GUIContent("Wrapping", "Wraps text to the next line when reaching the edge of the container.");
        static readonly GUIContent k_OverflowLabel = new GUIContent("Overflow", "How to display text which goes past the edge of the container.");

        void DrawFont() {
            bool isFontAssetDirty = false;

            // FONT ASSET
            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginDisabledGroup(style.objectReferenceValue != null);
            EditorGUILayout.PropertyField(m_FontAssetProp, k_FontAssetLabel);
            EditorGUI.EndDisabledGroup();
            if (EditorGUI.EndChangeCheck()) {
                m_HavePropertiesChanged = true;
                m_HasFontAssetChangedProp.boolValue = true;

                // Get new Material Presets for the new font asset
                m_MaterialPresetNames = GetMaterialPresets();
                m_MaterialPresetSelectionIndex = 0;

                isFontAssetDirty = true;
            }

            Rect rect;

            // MATERIAL PRESET
            if (m_MaterialPresetNames != null && !isFontAssetDirty) {
                EditorGUI.BeginChangeCheck();
                EditorGUI.BeginDisabledGroup(style.objectReferenceValue != null);
                rect = EditorGUILayout.GetControlRect(false, 17);

                EditorGUI.BeginProperty(rect, k_MaterialPresetLabel, m_FontSharedMaterialProp);

                float oldHeight = EditorStyles.popup.fixedHeight;
                EditorStyles.popup.fixedHeight = rect.height;

                int oldSize = EditorStyles.popup.fontSize;
                EditorStyles.popup.fontSize = 11;

                if (m_FontSharedMaterialProp.objectReferenceValue != null)
                    m_MaterialPresetIndexLookup.TryGetValue(m_FontSharedMaterialProp.objectReferenceValue.GetInstanceID(), out m_MaterialPresetSelectionIndex);

                m_MaterialPresetSelectionIndex = EditorGUI.Popup(rect, k_MaterialPresetLabel, m_MaterialPresetSelectionIndex, m_MaterialPresetNames);

                EditorGUI.EndProperty();
                EditorGUI.EndDisabledGroup();
                if (EditorGUI.EndChangeCheck()) {
                    m_FontSharedMaterialProp.objectReferenceValue = m_MaterialPresets[m_MaterialPresetSelectionIndex];
                    m_HavePropertiesChanged = true;
                }

                EditorStyles.popup.fixedHeight = oldHeight;
                EditorStyles.popup.fontSize = oldSize;
            }

            // FONT STYLE
            EditorGUI.BeginChangeCheck();

            int v1, v2, v3, v4, v5, v6, v7;

            if (EditorGUIUtility.wideMode) {
                rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight + 2f);

                EditorGUI.BeginProperty(rect, k_FontStyleLabel, m_FontStyleProp);

                EditorGUI.PrefixLabel(rect, k_FontStyleLabel);

                int styleValue = m_FontStyleProp.intValue;

                rect.x += EditorGUIUtility.labelWidth;
                rect.width -= EditorGUIUtility.labelWidth;

                rect.width = Mathf.Max(25f, rect.width / 7f);

                v1 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 1) == 1, k_BoldLabel, TMP_UIStyleManager.alignmentButtonLeft) ? 1 : 0; // Bold
                rect.x += rect.width;
                v2 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 2) == 2, k_ItalicLabel, TMP_UIStyleManager.alignmentButtonMid) ? 2 : 0; // Italics
                rect.x += rect.width;
                v3 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 4) == 4, k_UnderlineLabel, TMP_UIStyleManager.alignmentButtonMid) ? 4 : 0; // Underline
                rect.x += rect.width;
                v7 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 64) == 64, k_StrikethroughLabel, TMP_UIStyleManager.alignmentButtonRight) ? 64 : 0; // Strikethrough
                rect.x += rect.width;

                int selected = 0;

                EditorGUI.BeginChangeCheck();
                v4 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 8) == 8, k_LowercaseLabel, TMP_UIStyleManager.alignmentButtonLeft) ? 8 : 0; // Lowercase
                if (EditorGUI.EndChangeCheck() && v4 > 0) {
                    selected = v4;
                }
                rect.x += rect.width;
                EditorGUI.BeginChangeCheck();
                v5 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 16) == 16, k_UppercaseLabel, TMP_UIStyleManager.alignmentButtonMid) ? 16 : 0; // Uppercase
                if (EditorGUI.EndChangeCheck() && v5 > 0) {
                    selected = v5;
                }
                rect.x += rect.width;
                EditorGUI.BeginChangeCheck();
                v6 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 32) == 32, k_SmallcapsLabel, TMP_UIStyleManager.alignmentButtonRight) ? 32 : 0; // Smallcaps
                if (EditorGUI.EndChangeCheck() && v6 > 0) {
                    selected = v6;
                }

                if (selected > 0) {
                    v4 = selected == 8 ? 8 : 0;
                    v5 = selected == 16 ? 16 : 0;
                    v6 = selected == 32 ? 32 : 0;
                }

                EditorGUI.EndProperty();
            }
            else {
                rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight + 2f);

                EditorGUI.BeginProperty(rect, k_FontStyleLabel, m_FontStyleProp);

                EditorGUI.PrefixLabel(rect, k_FontStyleLabel);

                int styleValue = m_FontStyleProp.intValue;

                rect.x += EditorGUIUtility.labelWidth;
                rect.width -= EditorGUIUtility.labelWidth;
                rect.width = Mathf.Max(25f, rect.width / 4f);

                v1 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 1) == 1, k_BoldLabel, TMP_UIStyleManager.alignmentButtonLeft) ? 1 : 0; // Bold
                rect.x += rect.width;
                v2 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 2) == 2, k_ItalicLabel, TMP_UIStyleManager.alignmentButtonMid) ? 2 : 0; // Italics
                rect.x += rect.width;
                v3 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 4) == 4, k_UnderlineLabel, TMP_UIStyleManager.alignmentButtonMid) ? 4 : 0; // Underline
                rect.x += rect.width;
                v7 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 64) == 64, k_StrikethroughLabel, TMP_UIStyleManager.alignmentButtonRight) ? 64 : 0; // Strikethrough

                rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight + 2f);

                rect.x += EditorGUIUtility.labelWidth;
                rect.width -= EditorGUIUtility.labelWidth;

                rect.width = Mathf.Max(25f, rect.width / 4f);

                int selected = 0;

                EditorGUI.BeginChangeCheck();
                v4 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 8) == 8, k_LowercaseLabel, TMP_UIStyleManager.alignmentButtonLeft) ? 8 : 0; // Lowercase
                if (EditorGUI.EndChangeCheck() && v4 > 0) {
                    selected = v4;
                }
                rect.x += rect.width;
                EditorGUI.BeginChangeCheck();
                v5 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 16) == 16, k_UppercaseLabel, TMP_UIStyleManager.alignmentButtonMid) ? 16 : 0; // Uppercase
                if (EditorGUI.EndChangeCheck() && v5 > 0) {
                    selected = v5;
                }
                rect.x += rect.width;
                EditorGUI.BeginChangeCheck();
                v6 = TMP_EditorUtility.EditorToggle(rect, (styleValue & 32) == 32, k_SmallcapsLabel, TMP_UIStyleManager.alignmentButtonRight) ? 32 : 0; // Smallcaps
                if (EditorGUI.EndChangeCheck() && v6 > 0) {
                    selected = v6;
                }

                if (selected > 0) {
                    v4 = selected == 8 ? 8 : 0;
                    v5 = selected == 16 ? 16 : 0;
                    v6 = selected == 32 ? 32 : 0;
                }

                EditorGUI.EndProperty();
            }

            if (EditorGUI.EndChangeCheck()) {
                m_FontStyleProp.intValue = v1 + v2 + v3 + v4 + v5 + v6 + v7;
                m_HavePropertiesChanged = true;
            }

            // FONT SIZE
            EditorGUI.BeginChangeCheck();

            EditorGUI.BeginDisabledGroup(m_AutoSizingProp.boolValue);
            EditorGUILayout.PropertyField(m_FontSizeProp, k_FontSizeLabel, GUILayout.MaxWidth(EditorGUIUtility.labelWidth + 50f));
            EditorGUI.EndDisabledGroup();

            if (EditorGUI.EndChangeCheck()) {
                float fontSize = Mathf.Clamp(m_FontSizeProp.floatValue, 0, 32767);

                m_FontSizeProp.floatValue = fontSize;
                m_FontSizeBaseProp.floatValue = fontSize;
                m_HavePropertiesChanged = true;
            }

            EditorGUI.indentLevel += 1;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_AutoSizingProp, k_AutoSizeLabel);
            if (EditorGUI.EndChangeCheck()) {
                if (m_AutoSizingProp.boolValue == false)
                    m_FontSizeProp.floatValue = m_FontSizeBaseProp.floatValue;

                m_HavePropertiesChanged = true;
            }

            // Show auto sizing options
            if (m_AutoSizingProp.boolValue) {
                rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);

                EditorGUI.PrefixLabel(rect, k_AutoSizeOptionsLabel);

                int previousIndent = EditorGUI.indentLevel;

                EditorGUI.indentLevel = 0;

                rect.width = (rect.width - EditorGUIUtility.labelWidth) / 4f;
                rect.x += EditorGUIUtility.labelWidth;

                EditorGUIUtility.labelWidth = 24;
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(rect, m_FontSizeMinProp, k_MinLabel);
                if (EditorGUI.EndChangeCheck()) {
                    float minSize = m_FontSizeMinProp.floatValue;

                    minSize = Mathf.Max(0, minSize);

                    m_FontSizeMinProp.floatValue = Mathf.Min(minSize, m_FontSizeMaxProp.floatValue);
                    m_HavePropertiesChanged = true;
                }
                rect.x += rect.width;

                EditorGUIUtility.labelWidth = 27;
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(rect, m_FontSizeMaxProp, k_MaxLabel);
                if (EditorGUI.EndChangeCheck()) {
                    float maxSize = Mathf.Clamp(m_FontSizeMaxProp.floatValue, 0, 32767);

                    m_FontSizeMaxProp.floatValue = Mathf.Max(m_FontSizeMinProp.floatValue, maxSize);
                    m_HavePropertiesChanged = true;
                }
                rect.x += rect.width;

                EditorGUI.BeginChangeCheck();
                EditorGUIUtility.labelWidth = 36;
                EditorGUI.PropertyField(rect, m_CharWidthMaxAdjProp, k_WdLabel);
                rect.x += rect.width;
                EditorGUIUtility.labelWidth = 28;
                EditorGUI.PropertyField(rect, m_LineSpacingMaxProp, k_LineLabel);

                EditorGUIUtility.labelWidth = 0;

                if (EditorGUI.EndChangeCheck()) {
                    m_CharWidthMaxAdjProp.floatValue = Mathf.Clamp(m_CharWidthMaxAdjProp.floatValue, 0, 50);
                    m_LineSpacingMaxProp.floatValue = Mathf.Min(0, m_LineSpacingMaxProp.floatValue);
                    m_HavePropertiesChanged = true;
                }

                EditorGUI.indentLevel = previousIndent;
            }

            EditorGUI.indentLevel -= 1;



            EditorGUILayout.Space();
        }

        void DrawColor() {
            // FACE VERTEX COLOR
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_FontColorProp, k_BaseColorLabel);
            if (EditorGUI.EndChangeCheck()) {
                m_HavePropertiesChanged = true;
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_EnableVertexGradientProp, k_ColorGradientLabel);
            if (EditorGUI.EndChangeCheck()) {
                m_HavePropertiesChanged = true;
            }

            EditorGUIUtility.fieldWidth = 0;

            if (m_EnableVertexGradientProp.boolValue) {
                EditorGUI.indentLevel += 1;

                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(m_FontColorGradientPresetProp, k_ColorPresetLabel);

                SerializedObject obj = null;

                SerializedProperty colorMode;

                SerializedProperty topLeft;
                SerializedProperty topRight;
                SerializedProperty bottomLeft;
                SerializedProperty bottomRight;

                if (m_FontColorGradientPresetProp.objectReferenceValue == null) {
                    colorMode = m_ColorModeProp;
                    topLeft = m_FontColorGradientProp.FindPropertyRelative("topLeft");
                    topRight = m_FontColorGradientProp.FindPropertyRelative("topRight");
                    bottomLeft = m_FontColorGradientProp.FindPropertyRelative("bottomLeft");
                    bottomRight = m_FontColorGradientProp.FindPropertyRelative("bottomRight");
                }
                else {
                    obj = new SerializedObject(m_FontColorGradientPresetProp.objectReferenceValue);
                    colorMode = obj.FindProperty("colorMode");
                    topLeft = obj.FindProperty("topLeft");
                    topRight = obj.FindProperty("topRight");
                    bottomLeft = obj.FindProperty("bottomLeft");
                    bottomRight = obj.FindProperty("bottomRight");
                }

                EditorGUILayout.PropertyField(colorMode, k_ColorModeLabel);

                Rect rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2));

                EditorGUI.PrefixLabel(rect, k_CorenerColorsLabel);

                rect.x += EditorGUIUtility.labelWidth;
                rect.width = rect.width - EditorGUIUtility.labelWidth;

                switch ((ColorMode)colorMode.enumValueIndex) {
                    case ColorMode.Single:
                        TMP_EditorUtility.DrawColorProperty(rect, topLeft);

                        topRight.colorValue = topLeft.colorValue;
                        bottomLeft.colorValue = topLeft.colorValue;
                        bottomRight.colorValue = topLeft.colorValue;
                        break;
                    case ColorMode.HorizontalGradient:
                        rect.width /= 2f;

                        TMP_EditorUtility.DrawColorProperty(rect, topLeft);

                        rect.x += rect.width;

                        TMP_EditorUtility.DrawColorProperty(rect, topRight);

                        bottomLeft.colorValue = topLeft.colorValue;
                        bottomRight.colorValue = topRight.colorValue;
                        break;
                    case ColorMode.VerticalGradient:
                        TMP_EditorUtility.DrawColorProperty(rect, topLeft);

                        rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2));
                        rect.x += EditorGUIUtility.labelWidth;

                        TMP_EditorUtility.DrawColorProperty(rect, bottomLeft);

                        topRight.colorValue = topLeft.colorValue;
                        bottomRight.colorValue = bottomLeft.colorValue;
                        break;
                    case ColorMode.FourCornersGradient:
                        rect.width /= 2f;

                        TMP_EditorUtility.DrawColorProperty(rect, topLeft);

                        rect.x += rect.width;

                        TMP_EditorUtility.DrawColorProperty(rect, topRight);

                        rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2));
                        rect.x += EditorGUIUtility.labelWidth;
                        rect.width = (rect.width - EditorGUIUtility.labelWidth) / 2f;

                        TMP_EditorUtility.DrawColorProperty(rect, bottomLeft);

                        rect.x += rect.width;

                        TMP_EditorUtility.DrawColorProperty(rect, bottomRight);
                        break;
                }

                if (EditorGUI.EndChangeCheck()) {
                    m_HavePropertiesChanged = true;
                    if (obj != null) {
                        obj.ApplyModifiedProperties();
                        TMPro_EventManager.ON_COLOR_GRADIENT_PROPERTY_CHANGED(m_FontColorGradientPresetProp.objectReferenceValue as TMP_ColorGradient);
                    }
                }

                EditorGUI.indentLevel -= 1;
            }

            EditorGUILayout.PropertyField(m_OverrideHtmlColorProp, k_OverrideTagsLabel);

            EditorGUILayout.Space();
        }

        void DrawSpacing() {
            // CHARACTER, LINE & PARAGRAPH SPACING
            EditorGUI.BeginChangeCheck();

            Rect rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);

            EditorGUI.PrefixLabel(rect, k_SpacingOptionsLabel);

            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float currentLabelWidth = EditorGUIUtility.labelWidth;
            rect.x += currentLabelWidth;
            rect.width = (rect.width - currentLabelWidth - 3f) / 2f;

            EditorGUIUtility.labelWidth = Mathf.Min(rect.width * 0.55f, 80f);

            EditorGUI.PropertyField(rect, m_CharacterSpacingProp, k_CharacterSpacingLabel);
            rect.x += rect.width + 3f;
            EditorGUI.PropertyField(rect, m_WordSpacingProp, k_WordSpacingLabel);

            rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);

            rect.x += currentLabelWidth;
            rect.width = (rect.width - currentLabelWidth - 3f) / 2f;
            EditorGUIUtility.labelWidth = Mathf.Min(rect.width * 0.55f, 80f);

            EditorGUI.PropertyField(rect, m_LineSpacingProp, k_LineSpacingLabel);
            rect.x += rect.width + 3f;
            EditorGUI.PropertyField(rect, m_ParagraphSpacingProp, k_ParagraphSpacingLabel);

            EditorGUIUtility.labelWidth = currentLabelWidth;
            EditorGUI.indentLevel = oldIndent;

            if (EditorGUI.EndChangeCheck()) {
                m_HavePropertiesChanged = true;
            }

            EditorGUILayout.Space();
        }

        void DrawAlignment() {
            // TEXT ALIGNMENT
            EditorGUI.BeginChangeCheck();

            Rect rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.currentViewWidth > 504 ? 20 : 40 + 3);
            EditorGUI.BeginProperty(rect, k_AlignmentLabel, m_HorizontalAlignmentProp);
            EditorGUI.BeginProperty(rect, k_AlignmentLabel, m_VerticalAlignmentProp);

            EditorGUI.PrefixLabel(rect, k_AlignmentLabel);
            rect.x += EditorGUIUtility.labelWidth;

            EditorGUI.PropertyField(rect, m_HorizontalAlignmentProp, GUIContent.none);
            EditorGUI.PropertyField(rect, m_VerticalAlignmentProp, GUIContent.none);

            // WRAPPING RATIOS shown if Justified mode is selected.
            if (((HorizontalAlignmentOptions)m_HorizontalAlignmentProp.intValue & HorizontalAlignmentOptions.Justified) == HorizontalAlignmentOptions.Justified || ((HorizontalAlignmentOptions)m_HorizontalAlignmentProp.intValue & HorizontalAlignmentOptions.Flush) == HorizontalAlignmentOptions.Flush)
                DrawPropertySlider(k_WrapMixLabel, m_WordWrappingRatiosProp);

            if (EditorGUI.EndChangeCheck())
                m_HavePropertiesChanged = true;

            EditorGUI.EndProperty();
            EditorGUI.EndProperty();

            EditorGUILayout.Space();
        }
    }
}