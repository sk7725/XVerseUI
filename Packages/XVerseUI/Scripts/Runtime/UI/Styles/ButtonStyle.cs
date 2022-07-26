using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Selectable;

namespace XVerse.UI {
    [CreateAssetMenu(fileName = "NewButtonStyle", menuName = "XUI/Styles/ButtonStyle", order = 99)]
    public class ButtonStyle : ScriptableObject {
        public Sprite defaultSprite;
        public SpriteState sprites;
        public Image.Type imageType = Image.Type.Sliced;
        public Color defaultTextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);
        public float defaultTextFont = 24;

        public void Apply(XButton b) {
            b.background.sprite = defaultSprite;
            b.background.type = imageType;
            b.transition = Transition.SpriteSwap;
            b.spriteState = sprites;
        }
    }
}