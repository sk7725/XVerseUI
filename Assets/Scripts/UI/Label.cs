using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XVerse.UI {
    [AddComponentMenu("XUI/Label")]
    public class Label : TextMeshProUGUI, ILabel {
        public RectTransform rect;
        public LayoutElement cell;
        private Action updater = null;
        [HideInInspector]
        public bool overrideFontMaterial = false;

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

        public Label Material(Material mat) {
            fontSharedMaterial = mat;
            overrideFontMaterial = true;
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

        protected override void Awake() {
            base.Awake();
            rect = GetComponent<RectTransform>();
        }

        protected virtual void LateUpdate() {
            if (updater != null) updater();
        }
    }
}
