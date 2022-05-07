using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XTown.UI {
    public interface IElement<T> where T : Component {
        RectTransform GetRect();

        Color GetColor();
        T Color(Color color);

        T Updates(Action u);

        void SetScene(Canvas scene);
    }
}