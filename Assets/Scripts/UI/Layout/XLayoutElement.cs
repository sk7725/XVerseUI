using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XTown.UI {
    [AddComponentMenu("XUI/XLayout Element", 21)]
    public class XLayoutElement : LayoutElement {
        public bool endRow = false;
        public int colspan = 1;
    }
}