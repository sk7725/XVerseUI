using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XVerse.UI {
    [CreateAssetMenu(fileName = "NewBgStyle", menuName = "XUI/Styles/BackgroundStyle", order = 100)]
    public class BackgroundStyle : ScriptableObject {
        public Sprite sprite;
        public Color color;
        public Material material;
        public Image.Type imageType = Image.Type.Sliced;

        public void Apply(Background b) {
            b.sprite = sprite;
            b.type = imageType;
            b.material = material;
            if(!b.overrideColor) b.color = color;
        }
    }
}