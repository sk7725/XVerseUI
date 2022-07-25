using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XVerse.UI {
    [CreateAssetMenu(fileName = "NewLabelStyle", menuName = "XUI/Styles/LabelStyle", order = 101)]
    public class LabelStyle : ScriptableObject {
        public TMP_FontAsset font;
        public Material material;
        public TextOverflowModes overflow = TextOverflowModes.Overflow;
        public bool wrap = true;
        public Color defaultTextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);
        public float defaultTextFont = 36;

        public void Apply(Label l) {
            l.font = font;
            l.fontSharedMaterial = material;
            if(!l.overrideOverflow) l.overflowMode = overflow;
            if(!l.overrideWrap) l.enableWordWrapping = wrap;
        }

        public void ApplyDefaults(Label l) {
            l.fontSize = defaultTextFont;
            l.color = defaultTextColor;
        }
    }
}