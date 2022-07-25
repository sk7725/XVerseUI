using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace XVerse.UI {
    [CreateAssetMenu(fileName = "NewLabelStyle", menuName = "XUI/Styles/InputFieldStyle", order = 102)]
    public class InputFieldStyle : ScriptableObject {
        public TMP_FontAsset font;
        public Color defaultTextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);
        public float defaultTextFont = 24;

        public void Apply(XInputField l) {
            if(l.textComponent != null) l.textComponent.font = font;
            if(l.placeholder is TMP_Text t) t.font = font;
        }
    }
}