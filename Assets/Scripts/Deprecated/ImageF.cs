using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XTown.UI {
    /** Image frame. Holds Unity.UI.RawImage. */
    [RequireComponent(typeof(UnityEngine.UI.RawImage))]
    public class ImageF : Element {
        public UnityEngine.UI.RawImage image;

        protected override void Awake() {
            base.Awake();
            image = GetComponent<UnityEngine.UI.RawImage>();
        }

        public static new ImageF New() {
            ImageF im = _newObject<ImageF>();
            im.image = im.gameObject.GetComponent<UnityEngine.UI.RawImage>();
            return im;
        }

        public static ImageF New(string sprite) {
            ImageF im = New();
            im.image.texture = Resources.Load<Texture2D>("Sprites/"+sprite);
            return im;
        }

        public override void SetColor(Color color) {
            base.SetColor(color);
            image.color = color;
        }
    }
}
