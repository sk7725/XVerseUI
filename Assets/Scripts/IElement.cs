using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XTown.UI {
    public interface IElement<T> where T : Component {
        public RectTransform GetRect();

        public Color GetColor();
        public T Color(Color color);

        public T Updates(Action u);

        public void SetScene(Canvas scene);
    }
}