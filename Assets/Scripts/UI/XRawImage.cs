using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XTown.UI {
    [AddComponentMenu("XUI/XRawImage")]
    public class XRawImage : RawImage, IElement<XRawImage> {
        public RectTransform rect;
        private Action updater = null;

        public static XRawImage New() {
            GameObject go = new GameObject();
            XRawImage e = go.AddComponent<XRawImage>();
            e.rect = go.GetComponent<RectTransform>();
            return e;
        }

        public static XRawImage New(string sprite) {
            XRawImage im = New();
            im.texture = Resources.Load<Texture2D>("Sprites/" + sprite);
            return im;
        }

        public static XRawImage New(Texture2D sprite) {
            XRawImage im = New();
            im.texture = sprite;
            return im;
        }

        public XRawImage Color(Color color) {
            this.color = color;
            return this;
        }

        public Color GetColor() {
            return color;
        }

        public RectTransform GetRect() {
            return rect;
        }


        public void SetScene(Canvas scene) {
            rect.SetParent(scene.transform, false);
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
