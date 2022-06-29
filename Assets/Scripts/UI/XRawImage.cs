using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XVerse.UI {
    [AddComponentMenu("XUI/XRawImage")]
    public class XRawImage : RawImage, IElement<XRawImage> {
        public RectTransform rect;
        public XLayoutElement cell;
        private Action updater = null;

        private static XRawImage NewNamed(string name) {
            GameObject go = new GameObject(name);
            XRawImage e = go.AddComponent<XRawImage>();
            e.rect = go.GetComponent<RectTransform>();
            return e;
        }

        public static XRawImage New() {
            return NewNamed("xRawImage");
        }

        public static XRawImage New(string sprite) {
            XRawImage im = NewNamed(sprite);
            im.texture = Resources.Load<Texture2D>("Sprites/" + sprite);
            return im;
        }

        public static XRawImage New(Texture2D sprite) {
            XRawImage im = NewNamed(sprite.name);
            im.texture = sprite;
            return im;
        }

        public XRawImage Color(Color color) {
            this.color = color;
            return this;
        }

        public IElement<XRawImage> Get() {
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

        public XRawImage Updates(Action u) {
            updater = u;
            return this;
        }

        protected override void Awake() {
            base.Awake();
            rect = GetComponent<RectTransform>();
        }

        protected virtual void LateUpdate() {
            if(updater != null) updater();
        }
    }
}
