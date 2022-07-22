using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XVerse.UI {
    [AddComponentMenu("XUI/XInputField", 6)]
    public class XInputField : TMP_InputField, IElement<XInputField>, IStyle<InputFieldStyle> {
        public RectTransform rect;
        public LayoutElement cell;
        private Action updater = null;
        public Action<string> listener = null;

        public InputFieldStyle style;
        [HideInInspector]
        public bool overridePlaceholder = false;

        private static XInputField NewNamed(string name) {
            GameObject root = new GameObject(name);
            XInputField e = root.AddComponent<XInputField>();
            e.rect = root.GetComponent<RectTransform>();
            if(e.rect == null) e.rect = root.AddComponent<RectTransform>();

            //sdfkjhfsdjngsdokjgdskjo
            GameObject textArea = Utils.NewUIObject("Text Area", root);
            GameObject childPlaceholder = Utils.NewUIObject("Placeholder", textArea);
            GameObject childText = Utils.NewUIObject("Text", textArea);

            Image image = root.AddComponent<Image>();
            image.type = Image.Type.Sliced;
            image.color = UnityEngine.Color.white;

            Utils.SetDefaultColorTransitionValues(e);

            RectMask2D rectMask = textArea.AddComponent<RectMask2D>();
#if UNITY_2019_4_OR_NEWER
            rectMask.padding = new Vector4(-8, -5, -8, -5);
#endif

            //text area
            RectTransform textAreaRectTransform = textArea.GetComponent<RectTransform>();
            textAreaRectTransform.anchorMin = Vector2.zero;
            textAreaRectTransform.anchorMax = Vector2.one;
            textAreaRectTransform.sizeDelta = Vector2.zero;
            textAreaRectTransform.offsetMin = new Vector2(10, 6);
            textAreaRectTransform.offsetMax = new Vector2(-10, -7);

            //text area children
            TextMeshProUGUI text = childText.AddComponent<TextMeshProUGUI>();
            text.text = "";
            text.enableWordWrapping = false;
            text.extraPadding = true;
            text.richText = true;
            text.fontSize = 14;
            text.color = Utils.defaultTextColor;

            TextMeshProUGUI placeholder = childPlaceholder.AddComponent<TextMeshProUGUI>();
            placeholder.text = "Enter text...";
            placeholder.fontSize = 14;
            placeholder.fontStyle = FontStyles.Italic;
            placeholder.enableWordWrapping = false;
            placeholder.extraPadding = true;

            Color placeholderColor = text.color;
            placeholderColor.a *= 0.5f;
            placeholder.color = placeholderColor;

            placeholder.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;

            Utils.FillElement(childText.GetComponent<RectTransform>());
            Utils.FillElement(childPlaceholder.GetComponent<RectTransform>());

            e.textViewport = textAreaRectTransform;
            e.textComponent = text;
            e.placeholder = placeholder;
            e.fontAsset = text.font;

            return e;
        }

        public static XInputField New(Action<string> listener) {
            XInputField e = NewNamed("XInputField");
            e.listener = listener;
            return e;
        }

        public XInputField Color(Color color) {
            textComponent.color = color;
            return this;
        }

        public IElement<XInputField> Get() {
            return this;
        }

        public Color GetColor() {
            return textComponent.color;
        }

        public RectTransform GetRect() {
            return rect;
        }

        public LayoutElement GetCell() {
            return cell;
        }

        public LayoutElement AddCell() {
            if (cell == null) cell = gameObject.AddComponent<LayoutElement>();
            return cell;
        }

        public XInputField Updates(Action u) {
            updater = u;
            return this;
        }

        void _Edited(string s) {
            if (listener != null) listener(s);
        }

        public InputFieldStyle GetStyle() {
            return style;
        }

        public void SetStyle(InputFieldStyle style) {
            this.style = style;

            style.Apply(this);
        }

        //IElement<T> methods
        protected override void Awake() {
            base.Awake();
            rect = GetComponent<RectTransform>();
            onValueChanged.AddListener(s => _Edited(s));
        }

        protected override void LateUpdate() {
            base.LateUpdate();
            if (updater != null) updater();
        }

#if UNITY_EDITOR
        //IStyle<T> methods
        protected override void OnValidate() {
            if (style != null) {
                style.Apply(this);
            }
            base.OnValidate();
        }

        protected override void Reset() {
            base.Reset();
            InputFieldStyle s = Styles.Default<InputFieldStyle>();
            if (s != null) {
                style = s;
                style.Apply(this);
            }
        }
#endif
    }
}
