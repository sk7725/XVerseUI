using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace XVerse.UI {
    [CreateAssetMenu(fileName = "NewLabelStyle", menuName = "XUI/Styles/InputFieldStyle", order = 102)]
    public class InputFieldStyle : ScriptableObject {
        public TMP_FontAsset font;

        public void Apply(XInputField l) {
            if(l.textComponent != null) l.textComponent.font = font;
            if(l.placeholder is TMP_Text t) t.font = font;
        }
    }
}