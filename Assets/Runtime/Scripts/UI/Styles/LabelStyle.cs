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

        public void Apply(Label l) {
            l.font = font;
            l.fontSharedMaterial = material;
            if(!l.overrideOverflow) l.overflowMode = overflow;
            if(!l.overrideWrap) l.enableWordWrapping = wrap;
        }
    }
}