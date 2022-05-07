using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XTown.UI {
    [CustomEditor(typeof(XButton))]
    public class XButtonEditor : Editor {
        float thumbnailWidth = 70;
        float thumbnailHeight = 70;
        float labelWidth = 150f;

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
        }
    }
}