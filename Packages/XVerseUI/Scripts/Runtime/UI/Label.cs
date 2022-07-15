using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XVerse.UI {
    [AddComponentMenu("XUI/Label", 5)]
    public class Label : TextMeshProUGUI, ILabel, IStyle<LabelStyle> {
        public RectTransform rect;
        public LayoutElement cell;
        private Action updater = null;

        public LabelStyle style;
        [HideInInspector]
        public bool overrideOverflow = false, overrideWrap = false;

        //todo override Align to also align text - wrapper implementation
        private static Label NewNamed(string name) {
            GameObject go = new GameObject(name);
            Label e = go.AddComponent<Label>();
            e.rect = go.GetComponent<RectTransform>();
            return e;
        }

        public static Label New() {
            return NewNamed("label");
        }

        public static Label New(string text) {
            Label l = NewNamed(text);
            l.text = text;
            return l;
        }

        public static Label New(Func<string> textp) {
            Label l = NewNamed(textp());
            l.text = "";
            l.Updates(() => l.text = textp());
            return l;
        }

        public Label Wrap(bool wrap) {
            enableWordWrapping = wrap;
            overrideWrap = true;
            return this;
        }
        public Label Overflow(TextOverflowModes o) {
            overflowMode = o;
            overrideOverflow = true;
            return this;
        }

        public Label GetLabel() {
            return this;
        }

        public Label Color(Color color) {
            this.color = color;
            return this;
        }

        public IElement<Label> Get() {
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

        public Label Updates(Action u) {
            updater = u;
            return this;
        }

        public LabelStyle GetStyle() {
            return style;
        }

        public void SetStyle(LabelStyle style) {
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
            LabelStyle s = Styles.Default<LabelStyle>();
            if (s != null) {
                style = s;
                style.Apply(this);
            }
        }
    }
}
