using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XTown.UI {
    /** <summary>Utility for instantiating groups. 
     * Horizontal/Vertical Layout Group support is temporary and may be removed in the future.</summary>
     */
    public static class LayoutUtils {
        public static LayoutHelper<T> NewLayout<T>(bool fitSize) where T : Component {
            T go = new GameObject("LayoutGroup").AddComponent<T>();
            if(fitSize) go.gameObject.AddComponent<ContentSizeFitter>();
            var t = new LayoutHelper<T>(go);
            t.Add(Background.New());
            return t;
        }

        public static LayoutHelper<T> NewLayout<T>(float width, float height) where T : Component {
            var t = NewLayout<T>(false);
            //todo set size
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

            public LayoutHelper(T component) {
                this.component = component;
            }

            public void Add(Component o) {
                Add(o.gameObject);
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
        }
    }
}