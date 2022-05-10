using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XTown.UI {
    [AddComponentMenu("XUI/Label")]
    public class Label : Text, IElement<Label> {
        public RectTransform rect;
        public XLayoutElement cell;
        private Action updater = null;

        //todo override Align to also align text
        public static Label New() {
            GameObject go = new GameObject();
            Label e = go.AddComponent<Label>();
            e.rect = go.GetComponent<RectTransform>();
            return e;
        }

        public static Label New(string text) {
            Label l = New();
            l.text = text;
            return l;
        }

        public static Label New(Func<string> textp) {
            Label l = New();
            l.text = "";
            l.Updates(() => l.text = textp());
            return l;
        }

        public Label Color(Color color) {
            this.color = color;
            return this;
        }

        public Color GetColor() {
            return color;
        }

        public RectTransform GetRect() {
            return rect;
        }

        public XLayoutElement GetCell() {
            return cell;
        }

        public XLayoutElement AddCell() {
            if (cell == null) cell = gameObject.AddComponent<XLayoutElement>();
            return cell;
        }

        public void SetScene(Canvas scene) {
            rect.SetParent(scene.transform, false);
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
