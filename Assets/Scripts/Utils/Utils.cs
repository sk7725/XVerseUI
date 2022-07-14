using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XVerse.UI {
    public static class Utils {
        public static Color defaultTextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

        public static void SetLayerRecursively(GameObject go, int layer) {
            go.layer = layer;
            Transform t = go.transform;
            for (int i = 0; i < t.childCount; i++)
                SetLayerRecursively(t.GetChild(i).gameObject, layer);
        }

        public static void SetParentAlign(GameObject child, GameObject parent) {
            if (parent == null)
                return;

            child.transform.SetParent(parent.transform, false);
            SetLayerRecursively(child, parent.layer);
        }

        //use for vanilla UI only!
        public static GameObject NewUIObject(string name, GameObject parent) {
            GameObject go = new GameObject(name);
            go.AddComponent<RectTransform>();
            SetParentAlign(go, parent);
            return go;
        }

        public static void SetDefaultColorTransitionValues(Selectable slider) {
            ColorBlock colors = slider.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor = new Color(0.521f, 0.521f, 0.521f);
        }

        public static void FillElement(RectTransform rect) {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.offsetMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);
        }
    }
}