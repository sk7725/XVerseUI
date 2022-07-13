using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XVerse.UI {
    [AddComponentMenu("XUI/XInputField", 6)]
    public class XInputField : TextMeshProUGUI, IElement<XInputField>, IStyle<InputFieldStyle> {
        public RectTransform rect;
        public LayoutElement cell;
        private Action updater = null;

        public InputFieldStyle style;
        [HideInInspector]
        public bool overridePlaceholder = false;

        public XInputField Color(Color color) {
            this.color = color;
            return this;
        }

        public IElement<XInputField> Get() {
            return this;
        }

        public Color GetColor() {
            return color;
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
        }

        protected virtual void LateUpdate() {
            if (updater != null) updater();
        }

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
    }
}
