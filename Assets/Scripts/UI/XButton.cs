using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XTown.UI {
    [AddComponentMenu("XUI/XButton")]
    public class XButton : Button {
        public RectTransform rect;
        private Action updater = null;
        //todo
        public struct ButtonStyle {
            //todo
        }

        protected override void Awake() {
            base.Awake();
            rect = GetComponent<RectTransform>();
        }
    }
}