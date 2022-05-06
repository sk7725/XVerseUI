using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XTown.UI {
    [AddComponentMenu("XUI/XImage")]
    public class XImage : Image, IElement<XImage> {
        public RectTransform rect;
        private Action updater = null;

        public static XImage New() {
            GameObject go = new GameObject();
            XImage e = go.AddComponent<XImage>();
            e.rect = go.GetComponent<RectTransform>();
            return e;
        }

        public static XImage New(string sprite) {
            XImage im = New();
            im.sprite = Resources.Load<Sprite>("Sprites/" + sprite);
            return im;
        }

        public static XImage New(Sprite sprite) {
            XImage im = New();
            im.sprite = sprite;
            return im;
        }

        public XImage Color(Color color) {
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
