using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XTown.UI {
    [AddComponentMenu("XUI/XButton")] [RequireComponent(typeof(XImage))]
    public class XButton : Button, IElement<XButton>, IGroup, IStyle<ButtonStyle>{
        public RectTransform rect;
        public XLayoutElement cell = null;
        public XImage background;

        public bool pressed, hovering;

        private Action updater = null;
        private Action clicked = null;
        public ButtonStyle style = null;//todo editorscript -> CustomEditor(typeof(XButton))

        //New(string text..., Action clicked)
        //Size()

        public static XButton New() {
            GameObject go = new GameObject();
            XButton e = go.AddComponent<XButton>();
            e.rect = go.GetComponent<RectTransform>();
            e.style = Styles.Default<ButtonStyle>();
            return e;
        }

        public static XButton New(Action clicked) {
            XButton b = New();
            b.clicked = clicked;
            return b;
        }

        public XButton Color(Color color) {
            background.Color(color);
            return this;
        }

        public Color GetColor() {
            return background.color;
        }

        public RectTransform GetRect() {
            return rect;
        }

        public XLayoutElement GetCell() {
            return cell;
        }

        public XLayoutElement AddCell() {
            if(cell == null) cell = gameObject.AddComponent<XLayoutElement>();
            return cell;
        }

        public void SetScene(Canvas scene) {
            rect.SetParent(scene.transform, false);
        }

        public XButton Updates(Action u) {
            updater = u;
            return this;
        }

        public XButton Clicked(Action c) {
            clicked = c;
            return this;
        }

        void _OnClicked() {
            if (clicked != null) clicked();
        }

        public ButtonStyle GetStyle() {
            return style;
        }

        public void SetStyle(ButtonStyle style) {
            this.style = style;

            style.Apply(this);
            //todo apply style
        }

        //todo

        protected override void Awake() {
            base.Awake();
            rect = GetComponent<RectTransform>();
            background = GetComponent<XImage>();
            onClick.AddListener(_OnClicked);
        }

        protected virtual void LateUpdate() {
            if (updater != null) updater();
        }

        protected override void OnValidate() {
            base.OnValidate();
            if (style != null) {
                style.Apply(this);
            }
        }

        public override void OnPointerEnter(PointerEventData eventData) {
            base.OnPointerEnter(eventData);
            hovering = true;
        }

        public override void OnPointerDown(PointerEventData eventData) {
            base.OnPointerDown(eventData);
            pressed = true;
        }

        public override void OnPointerUp(PointerEventData eventData) {
            base.OnPointerUp(eventData);
            pressed = false;
        }

        public override void OnPointerExit(PointerEventData eventData) {
            base.OnPointerExit(eventData);
            pressed = false;
            hovering = false;
        }
    }
}