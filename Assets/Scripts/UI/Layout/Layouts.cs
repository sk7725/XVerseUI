using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XVerse.UI {
    /** <summary>Utility for instantiating groups.</summary>
     */
    public static class Layouts {
        public static LayoutHelper<T> NewLayout<T>(bool fitSize) where T : LayoutGroup {
            T go = new GameObject("LayoutGroup").AddComponent<T>();
            go.childAlignment = TextAnchor.MiddleCenter;
            if (fitSize) {
                ContentSizeFitter csf = go.gameObject.AddComponent<ContentSizeFitter>();
                csf.horizontalFit = csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
            if(go is HorizontalOrVerticalLayoutGroup hv) {
                hv.childForceExpandHeight = hv.childForceExpandWidth = false;
            }
            Background e = go.gameObject.AddComponent<Background>();
            e.style = Styles.Default<BackgroundStyle>();
            e.style.Apply(e);

            var t = new LayoutHelper<T>(go, e);
            return t;
        }

        public static LayoutHelper<T> NewLayout<T>(float width, float height) where T : LayoutGroup {
            var t = NewLayout<T>(false);
            t.Get().Size(width, height);
            return t;
        }

        public static LayoutHelper<T> NewLayout<T>() where T : LayoutGroup {
            return NewLayout<T>(false);
        }

        public static LayoutHelper<HorizontalLayoutGroup> Horizontal() {
            return NewLayout<HorizontalLayoutGroup>(false);
        }

        public static LayoutHelper<VerticalLayoutGroup> Vertical() {
            return NewLayout<VerticalLayoutGroup>(false);
        }

        public static LayoutHelper<HorizontalLayoutGroup> Horizontal(bool fitSize) {
            return NewLayout<HorizontalLayoutGroup>(fitSize);
        }

        public static LayoutHelper<VerticalLayoutGroup> Vertical(bool fitSize) {
            return NewLayout<VerticalLayoutGroup>(fitSize);
        }

        public static void ClearChildren(GameObject o) {
            int n = o.transform.childCount;
            if (n <= 0) return;
            for (int i = n - 1; i >= 0; i--) {
                UnityEngine.Object.Destroy(o.transform.GetChild(i).gameObject);
            }
        }

        public class LayoutHelper<T> where T : LayoutGroup {
            public T component;
            public XImage element;

            public LayoutHelper(T component, XImage element) {
                this.component = component;
                this.element = element;

                if (element.sprite != null) {
                    component.padding.left = (int)element.sprite.border.x;
                    component.padding.right = (int)element.sprite.border.z;
                    component.padding.top = (int)element.sprite.border.w;
                    component.padding.bottom = (int)element.sprite.border.y;
                }
            }

            public void Add(Component o) {
                Add(o.gameObject);
            }

            public void Add(Component o, int index) {
                Add(o.gameObject, index);
            }

            public void Add(GameObject o) {
                o.transform.SetParent(component.transform, false);
            }

            public void Add(GameObject o, int index) {
                o.transform.SetParent(component.transform, false);
                o.transform.SetSiblingIndex(Math.Clamp(index, 0, component.transform.childCount - 1));
            }

            public void ClearChildren() {
                Layouts.ClearChildren(component.gameObject);
            }

            public IElement<XImage> Get() {
                return element;
            }

            //utility methods
            public Label Add(string text) {
                Label l = Label.New(text);
                Add(l);
                return l;
            }

            public XImage Image(Sprite sprite) {
                XImage image = XImage.New(sprite);
                Add(image);
                return image;
            }

            public XRawImage RawImage() {
                XRawImage image = XRawImage.New();
                Add(image);
                return image;
            }

            public XRawImage RawImage(Texture2D tex) {
                XRawImage image = XRawImage.New(tex);
                Add(image);
                return image;
            }

            public XRawImage RawImage(Sprite sprite) {
                XRawImage image = XRawImage.New(sprite.texture);
                Add(image);
                return image;
            }

            public XButton Button<F>(Action<LayoutHelper<F>> cons, ButtonStyle style, Action clicked) where F: LayoutGroup {
                XButton b = XButton.NewStyled(style);
                b.Clicked(clicked);
                cons(b.AsLayout<F>());
                Add(b);
                return b;
            }

            public XButton Button<F>(Action<LayoutHelper<F>> cons, Action clicked) where F : LayoutGroup {
                return Button(cons, null, clicked);
            }

            public XButton Button(string text, Action clicked) {
                XButton b = XButton.New(text, clicked);
                Add(b);
                return b;
            }

            public XButton Button(string text, LabelStyle textStyle, Action clicked) {
                XButton b = XButton.New(text, textStyle, clicked);
                Add(b);
                return b;
            }

            public XButton Button(Texture2D image, float imageSize, Action clicked) {
                XButton b = XButton.New(image, imageSize, clicked);
                Add(b);
                return b;
            }

            public XButton Button(Texture2D image, Action clicked) {
                return Button(image, image.width, clicked);
            }

            public XButton Button(string text, Texture2D image, LabelStyle textStyle, float imageSize, Action clicked) {
                return Button<HorizontalLayoutGroup>(b => {
                    b.RawImage(image).Get().Size(imageSize).Center();
                    Label l = b.Add(text);
                    l.SetStyle(textStyle);
                    l.Get().Grow().Center();
                }, clicked);
            }

            public XButton Button(string text, Texture2D image, float imageSize, Action clicked) {
                return Button<HorizontalLayoutGroup>(b => {
                    b.RawImage(image).Get().Size(imageSize).Center();
                    b.Add(text).Get().Grow().Center();
                }, clicked);
            }

            public XButton Button(string text, Texture2D image, Action clicked) {
                return Button(text, image, image.width, clicked);
            }

            //todo Button(text, image, textStyle, imageSize, clicked)

            public IElement<XImage> SubLayout<F>(Action<LayoutHelper<F>> cons) where F: LayoutGroup {
                LayoutHelper<F> t = NewLayout<F>(false);
                cons(t);
                Add(t.component.gameObject);
                return t.Get();
            }

            public IElement<XImage> Vertical(Action<LayoutHelper<VerticalLayoutGroup>> cons) {
                return SubLayout(cons);
            }

            public IElement<XImage> Horizontal(Action<LayoutHelper<HorizontalLayoutGroup>> cons) {
                return SubLayout(cons);
            }

            //alignment
            public LayoutHelper<T> Center() {
                component.childAlignment = TextAnchor.MiddleCenter;
                return this;
            }

            //the person who made the goddamn anchor non-bitwise should go to hell
            //0 1 2
            //3 4 5
            //6 7 8
            public LayoutHelper<T> Left() {
                component.childAlignment = (TextAnchor)((int)component.childAlignment / 3 * 3);
                return this;
            }

            public LayoutHelper<T> Right() {
                component.childAlignment = (TextAnchor)((int)component.childAlignment / 3 * 3 + 2);
                return this;
            }

            public LayoutHelper<T> Top() {
                component.childAlignment = (TextAnchor)((int)component.childAlignment % 3);
                return this;
            }

            public LayoutHelper<T> Bottom() {
                component.childAlignment = (TextAnchor)((int)component.childAlignment % 3 + 6);
                return this;
            }
        }
    }
}