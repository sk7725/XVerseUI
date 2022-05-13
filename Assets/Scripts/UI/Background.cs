using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XTown.UI {
    [AddComponentMenu("XUI/Background")]
    public class Background : XImage, IStyle<BackgroundStyle> {
        public BackgroundStyle style;
        [HideInInspector]
        public bool overrideColor = false;

        public static new Background New() {
            GameObject go = new GameObject("background");
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

        protected override void OnValidate() {
            base.OnValidate();
            if (style != null) {
                style.Apply(this);
            }
        }
    }
}