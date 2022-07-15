using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XVerse.UI {
    public interface IStyle<T> where T: ScriptableObject {
        void SetStyle(T style);
        T GetStyle();
    }
}