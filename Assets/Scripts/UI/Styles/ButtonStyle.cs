using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Selectable;

namespace XTown.UI {
    [CreateAssetMenu(fileName = "NewButtonStyle", menuName = "XUI/Styles/ButtonStyle", order = 99)]
    public class ButtonStyle : ScriptableObject {
        public Sprite defaultSprite;
        public SpriteState sprites;

        public void Apply(XButton b) {
            b.background.sprite = defaultSprite;
            b.transition = Transition.SpriteSwap;
            b.spriteState = sprites;
        }
    }
}