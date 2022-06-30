using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XVerse.UI {
    public class EmptyElement : MonoBehaviour, IElement<EmptyElement> {
        public RectTransform rect;
        public LayoutElement cell;
        private Action updater = null;

        public static EmptyElement New(string name) {
            GameObject go = new GameObject(name);
            EmptyElement e = go.AddComponent<EmptyElement>();
            e.rect = go.GetComponent<RectTransform>();
            return e;
        }

        public static EmptyElement New() {
            return New("Empty");
        }

        public EmptyElement Color(Color color) {
            return this;
        }

        public IElement<EmptyElement> Get() {
            return this;
        }

        public Color GetColor() {
            return UnityEngine.Color.white;
        }

        public RectTransform GetRect() {
            return rect;
        }

        public EmptyElement Updates(Action u) {
            updater = u;
            return this;
        }

        public LayoutElement GetCell() {
            return cell;
        }

        public LayoutElement AddCell() {
            if (cell == null) cell = gameObject.AddComponent<LayoutElement>();
            return cell;
        }

        protected void Awake() {
            rect = GetComponent<RectTransform>();
        }

        protected virtual void LateUpdate() {
            if (updater != null) updater();
        }
    }
}