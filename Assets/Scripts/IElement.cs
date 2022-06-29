using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XVerse.UI {
    public interface IElement<T> where T : Component {
        IElement<T> Get();

        RectTransform GetRect();

        XLayoutElement GetCell();

        XLayoutElement AddCell();

        Color GetColor();
        T Color(Color color);

        T Updates(Action u);

        public void SetScene(Canvas scene) {
            GetRect().SetParent(scene.transform, false);
        }

        public IElement<T> Center() {
            RectTransform r = GetRect();
            r.pivot = new Vector2(0.5f, 0.5f);
            r.anchorMin = new Vector2(0.5f, 0.5f);
            r.anchorMax = new Vector2(0.5f, 0.5f);
            return this;
        }

        public IElement<T> Up() {
            RectTransform r = GetRect();
            r.pivot = new Vector2(r.pivot.x, 1f);
            r.anchorMin = new Vector2(r.anchorMin.x, 1f);
            r.anchorMax = new Vector2(r.anchorMax.x, 1f);
            return this;
        }

        public IElement<T> Left() {
            RectTransform r = GetRect();
            r.pivot = new Vector2(0f, r.pivot.y);
            r.anchorMin = new Vector2(0f, r.anchorMin.y);
            r.anchorMax = new Vector2(0f, r.anchorMax.y);
            return this;
        }

        public IElement<T> Down() {
            RectTransform r = GetRect();
            r.pivot = new Vector2(r.pivot.x, 0f);
            r.anchorMin = new Vector2(r.anchorMin.x, 0f);
            r.anchorMax = new Vector2(r.anchorMax.x, 0f);
            return this;
        }

        public IElement<T> Right() {
            RectTransform r = GetRect();
            r.pivot = new Vector2(1f, r.pivot.y);
            r.anchorMin = new Vector2(1f, r.anchorMin.y);
            r.anchorMax = new Vector2(1f, r.anchorMax.y);
            return this;
        }

        public IElement<T> FillX() {
            RectTransform r = GetRect();
            r.pivot = new Vector2(0.5f, r.pivot.y);
            r.offsetMin = new Vector2(0f, r.offsetMin.y);
            r.offsetMax = new Vector2(0f, r.offsetMax.y);
            r.anchorMin = new Vector2(0f, r.anchorMin.y);
            r.anchorMax = new Vector2(1f, r.anchorMax.y);
            return this;
        }

        public IElement<T> FillY() {
            RectTransform r = GetRect();
            r.pivot = new Vector2(r.pivot.x, 0.5f);
            r.offsetMin = new Vector2(r.offsetMin.x, 0f);
            r.offsetMax = new Vector2(r.offsetMax.x, 0f);
            r.anchorMin = new Vector2(r.anchorMin.x, 0f);
            r.anchorMax = new Vector2(r.anchorMax.x, 1f);
            return this;
        }

        public IElement<T> Fill() {
            return FillX().FillY();
        }

        public IElement<T> Width(float amount) {
            GetRect().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, amount);
            return this;
        }

        public IElement<T> Height(float amount) {
            GetRect().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, amount);
            return this;
        }

        public IElement<T> Size(float size) {
            return Size(size, size);
        }

        public IElement<T> Size(float width, float height) {
            return Width(width).Height(height);
        }
    }
}