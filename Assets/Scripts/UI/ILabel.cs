using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Wrapper class for Label.
 * Exists because classes cannot override default methods of interfaces (Very Good Language Design)
 */

namespace XTown.UI {
    public interface ILabel : IElement<Label> {
        Label GetLabel();
        IElement<Label> IElement<Label>.Center() {
            RectTransform r = GetRect();
            r.pivot = new Vector2(0.5f, 0.5f);
            r.anchorMin = new Vector2(0.5f, 0.5f);
            r.anchorMax = new Vector2(0.5f, 0.5f);
            GetLabel().alignment = TMPro.TextAlignmentOptions.Center;
            return this;
        }

        IElement<Label> IElement<Label>.Up() {
            RectTransform r = GetRect();
            r.pivot = new Vector2(r.pivot.x, 1f);
            r.anchorMin = new Vector2(r.anchorMin.x, 1f);
            r.anchorMax = new Vector2(r.anchorMax.x, 1f);
            Label l = GetLabel();
            l.alignment = (TMPro.TextAlignmentOptions)((int)l.alignment & 0x00FF | (int)TMPro.VerticalAlignmentOptions.Top);
            return this;
        }

        IElement<Label> IElement<Label>.Left() {
            RectTransform r = GetRect();
            r.pivot = new Vector2(0f, r.pivot.y);
            r.anchorMin = new Vector2(0f, r.anchorMin.y);
            r.anchorMax = new Vector2(0f, r.anchorMax.y);
            Label l = GetLabel();
            l.alignment = (TMPro.TextAlignmentOptions)((int)l.alignment & 0xFF00 | (int)TMPro.HorizontalAlignmentOptions.Left);
            return this;
        }

        IElement<Label> IElement<Label>.Down() {
            RectTransform r = GetRect();
            r.pivot = new Vector2(r.pivot.x, 0f);
            r.anchorMin = new Vector2(r.anchorMin.x, 0f);
            r.anchorMax = new Vector2(r.anchorMax.x, 0f);
            Label l = GetLabel();
            l.alignment = (TMPro.TextAlignmentOptions)((int)l.alignment & 0x00FF | (int)TMPro.VerticalAlignmentOptions.Bottom);
            return this;
        }

        IElement<Label> IElement<Label>.Right() {
            RectTransform r = GetRect();
            r.pivot = new Vector2(1f, r.pivot.y);
            r.anchorMin = new Vector2(1f, r.anchorMin.y);
            r.anchorMax = new Vector2(1f, r.anchorMax.y);
            Label l = GetLabel();
            l.alignment = (TMPro.TextAlignmentOptions)((int)l.alignment & 0xFF00 | (int)TMPro.HorizontalAlignmentOptions.Right);
            return this;
        }
    }
}