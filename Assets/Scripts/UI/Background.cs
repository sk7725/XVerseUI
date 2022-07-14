using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XVerse.UI {
    [AddComponentMenu("XUI/Background", 4)]
    public class Background : XImage, IStyle<BackgroundStyle> {
        public BackgroundStyle style;
        [HideInInspector]
        public bool overrideColor = false;

        public static new Background New() {
            GameObject go = new GameObject("Background");
            Background e = go.AddComponent<Background>();
            e.rect = go.GetComponent<RectTransform>();
            e.style = Styles.Default<BackgroundStyle>();
            e.style.Apply(e);
            e.Get().Fill();
            return e;
        }

        public BackgroundStyle GetStyle() {
            return style;
        }

        public void SetStyle(BackgroundStyle style) {
            this.style = style;

            style.Apply(this);
        }

        public override XImage Color(Color color) {
            overrideColor = true;
            return base.Color(color);
        }

        //IStyle<T> methods
        protected override void OnValidate() {
            base.OnValidate();
            if (style != null) {
                style.Apply(this);
            }
        }

        protected override void Reset() {
            base.Reset();
            BackgroundStyle s = Styles.Default<BackgroundStyle>();
            if (s != null) {
                style = s;
                style.Apply(this);
            }
        }
    }
}