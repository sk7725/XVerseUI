using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XVerse.UI {
    [CreateAssetMenu(fileName = "NewLabelStyle", menuName = "XUI/Styles/LabelStyle", order = 100)]
    public class LabelStyle : ScriptableObject {
        public TMP_FontAsset font;
        public Material material;
        public TextOverflowModes overflow = TextOverflowModes.Overflow;
        public bool wrap = true;

        public void Apply(Label l) {
            l.font = font;
            if (!l.overrideFontMaterial) l.fontSharedMaterial = material;
            l.overflowMode = overflow;
            l.enableWordWrapping = wrap;
        }
    }
}