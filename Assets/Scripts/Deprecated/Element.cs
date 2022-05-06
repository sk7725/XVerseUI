using System;
using UnityEngine;

namespace XTown.UI {
    [RequireComponent(typeof(RectTransform))]
    public class Element : MonoBehaviour {
        public RectTransform rect;
        public Group parent;
        public Color color = Color.white;

        public float width, height; //do not modify without calling sizeChanged!
        public bool visible = true;
        public Canvas scene;

        private Action update;
        private Func<bool> visibility;
        //todo add listener system

        private bool needsLayout = true;

        protected virtual void Awake() {
            rect = GetComponent<RectTransform>();
            width = rect.localScale.x;
            height = rect.localScale.y;
        }

        protected virtual void Update() {
            if (parent == null) {
                UpdateBase();
            }
        }

        /** THis update is called by the parent recursively. */
        public virtual void UpdateBase() {
            if (update != null) update();
        }

        public Element Updates(Action u) {
            update = u;
            return this;
        }

        public Element Visible(Func<bool> u) {
            visibility = u;
            return this;
        }

        public void UpdateVisibility() {
            if (visibility != null) visible = visibility();
        }

        public static Element New() {
            return _newObject<Element>();
        }

        public void Remove() {
            Destroy(gameObject);
        }

        public virtual void SetScene(Canvas scene) {
            this.scene = scene;
            parent = null;
            rect.SetParent(scene.transform, false);
        }

        public void SetWidth(float width) {
            if (width != this.width) {
                this.width = width;
                SizeChanged();
            }
        }

        public void SetHeight(float height) {
            if (height != this.height) {
                this.height = height;
                SizeChanged();
            }
        }

        public void SetSize(float width, float height) {
            if (this.width != width || this.height != height) {
                this.width = width;
                this.height = height;
                SizeChanged();
            }
        }

        public void SizeChanged() {
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            needsLayout = true;
        }

        public float GetMinWidth() {
            return GetPrefWidth();
        }

        public float GetMinHeight() {
            return GetPrefHeight();
        }

        public float GetPrefWidth() {
            return 0;
        }

        public float GetPrefHeight() {
            return 0;
        }

        public float GetMaxWidth() {
            return 0;
        }

        public float GetMaxHeight() {
            return 0;
        }

        public int GetZIndex() {
            if (parent == null) return -1;
            return rect.GetSiblingIndex();
        }

        public void SetZIndex(int index) {
            if(parent == null) return;
            rect.SetSiblingIndex(Mathf.Clamp(index, 0, parent.rect.childCount - 1));
        }

        public void ToFront() {
            rect.SetAsLastSibling();
        }

        public void ToBack() {
            rect.SetAsFirstSibling();
        }

        public virtual void SetColor(Color color) {
            this.color = color;
        }

        //internal methods
        protected static T _newObject<T>() where T:Element {
            GameObject go = new GameObject();
            T e = go.AddComponent<T>();
            e.rect = go.GetComponent<RectTransform>();
            return e;
        }
    }
}