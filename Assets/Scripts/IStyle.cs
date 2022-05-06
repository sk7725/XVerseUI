using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XTown.UI {
    public interface IStyle<T> {
        public void SetStyle(T style);
        public T GetStyle();
    }
}