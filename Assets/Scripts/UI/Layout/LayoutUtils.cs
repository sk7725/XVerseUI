using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XVerse.UI {
    /** <summary>Utility for instantiating groups. 
     * Horizontal/Vertical Layout Group support is temporary and may be removed in the future.</summary>
     */
    public static class LayoutUtils {
        public static LayoutHelper<T> NewLayout<T>(bool fitSize) where T : Component {
            T go = new GameObject("LayoutGroup").AddComponent<T>();
            if(fitSize) go.gameObject.AddComponent<ContentSizeFitter>();
            Background e = go.gameObject.AddComponent<Background>();
            e.style = Styles.Default<BackgroundStyle>();
            e.style.Apply(e);

            var t = new LayoutHelper<T>(go, e);
            return t;
        }

        public static LayoutHelper<T> NewLayout<T>(float width, float height) where T : Component {
            var t = NewLayout<T>(false);
            t.Get().Size(width, height);
            return t;
        }

        public static LayoutHelper<T> NewLayout<T>() where T : Component {
            return NewLayout<T>(true);
        }

        public static void ClearChildren(GameObject o) {
            int n = o.transform.childCount;
            if (n <= 0) return;
            for (int i = n - 1; i >= 0; i--) {
                UnityEngine.Object.Destroy(o.transform.GetChild(i).gameObject);
            }
        }

        public class LayoutHelper<T> where T : Component {
            public T component;
            public Background element;

            public LayoutHelper(T component, Background element) {
                this.component = component;
                this.element = element;
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
                LayoutUtils.ClearChildren(component.gameObject);
            }

            public IElement<XImage> Get() {
                return element;
            }
        }
    }
}