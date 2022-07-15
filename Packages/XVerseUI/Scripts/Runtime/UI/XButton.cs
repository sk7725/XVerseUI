using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static XVerse.UI.Layouts;

namespace XVerse.UI {
    [AddComponentMenu("XUI/XButton", 1)] [RequireComponent(typeof(XImage))]
    public class XButton : Button, IElement<XButton>, IStyle<ButtonStyle>{
        public RectTransform rect;
        public LayoutElement cell = null;
        public XImage background;

        public bool pressed, hovering;

        private Action updater = null;
        private Action clicked = null;
        public ButtonStyle style = null;

        public static XButton NewStyled(ButtonStyle style) {
            GameObject go = new GameObject("XButton");
            XButton e = go.AddComponent<XButton>();
            e.rect = go.GetComponent<RectTransform>();
            e.style = style == null ? Styles.Default<ButtonStyle>() : style;
            e.style.Apply(e);
            return e;
        }

        public static XButton New() {
            return NewStyled(null);
        }

        public static XButton New(Action clicked) {
            XButton b = New();
            b.clicked = clicked;
            return b;
        }

        public static XButton New(string text, Action clicked) {
            XButton b = New(clicked);
            Label label = Label.New(text);
            label.fontSize = 24; //todo set to buttonstyle
            label.rect.SetParent(b.rect, false);
            label.Get().Center().Fill();
            return b;
        }

        public static XButton New(string text, LabelStyle textStyle, Action clicked) {
            XButton b = New(clicked);
            Label label = Label.New(text);
            label.fontSize = 24; //todo set to buttonstyle
            label.rect.SetParent(b.rect, false);
            label.SetStyle(textStyle);
            label.Get().Center().Fill();
            return b;
        }

        public static XButton New(Texture2D icon, float iconSize, Action clicked) {
            XButton b = New(clicked);
            XRawImage image = XRawImage.New(icon);
            image.rect.SetParent(b.rect, false);
            image.Get().Center().Size(iconSize);
            return b;
        }

        public XButton Color(Color color) {
            background.Color(color);
            return this;
        }

        public IElement<XButton> Get() {
            return this;
        }

        public Color GetColor() {
            return background.color;
        }

        public RectTransform GetRect() {
            return rect;
        }

        public LayoutElement GetCell() {
            return cell;
        }

        public LayoutElement AddCell() {
            if(cell == null) cell = gameObject.AddComponent<LayoutElement>();
            return cell;
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
        }

        public LayoutHelper<T> AsLayout<T>() where T: LayoutGroup {
            T c; gameObject.TryGetComponent(out c);
            if (c == null) {
                c = gameObject.AddComponent<T>();
                c.childAlignment = TextAnchor.MiddleCenter;
            }
            if(c is HorizontalOrVerticalLayoutGroup hv) {
                hv.childControlWidth = hv.childControlHeight = true;
                hv.childForceExpandHeight = hv.childForceExpandWidth = false;
            }

            return new LayoutHelper<T>(c, background);
        }

        //IElement<T> default methods
        protected override void Awake() {
            base.Awake();
            rect = GetComponent<RectTransform>();
            background = GetComponent<XImage>();
            onClick.AddListener(_OnClicked);
        }

        protected virtual void LateUpdate() {
            if (updater != null) updater();
        }

        //IStyle<T> default methods
        protected override void OnValidate() {
            base.OnValidate();
            if (style != null) {
                style.Apply(this);
            }
        }

        protected override void Reset() {
            base.Reset();
            ButtonStyle s = Styles.Default<ButtonStyle>();
            if (s != null) {
                style = s;
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