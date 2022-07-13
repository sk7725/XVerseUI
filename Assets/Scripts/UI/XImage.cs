using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XVerse.UI {
    [AddComponentMenu("XUI/XImage", 2)]
    public class XImage : Image, IElement<XImage> {
        public RectTransform rect;
        public LayoutElement cell;
        private Action updater = null;

        private static XImage NewNamed(string name) {
            GameObject go = new GameObject(name);
            XImage e = go.AddComponent<XImage>();
            e.rect = go.GetComponent<RectTransform>();
            return e;
        }

        public static XImage New() {
            return NewNamed("xImage");
        }

        public static XImage New(string sprite) {
            XImage im = NewNamed(sprite);

            im.sprite = Resources.Load<Sprite>("Sprites/" + sprite);
            return im;
        }

        public static XImage New(Sprite sprite) {
            XImage im = NewNamed(sprite.name);
            im.sprite = sprite;
            return im;
        }

        public virtual XImage Color(Color color) {
            this.color = color;
            return this;
        }

        public IElement<XImage> Get() {
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

        public XImage Updates(Action u) {
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
