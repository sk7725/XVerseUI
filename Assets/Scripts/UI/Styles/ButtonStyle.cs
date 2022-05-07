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
        public bool isDefault = false;

        private void OnValidate() {
            if (isDefault) {
                /*if (defaults != this) {
                    defaults.isDefault = false;
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(defaults);
#endif
                }
                defaults = this;*/
                //todo what is the best way of linking Styles? Should it be a gameobject or sc?
            }
        }

        public void Apply(XButton b) {
            b.background.sprite = defaultSprite;
            b.transition = Transition.SpriteSwap;
            b.spriteState = sprites;
        }
    }
}