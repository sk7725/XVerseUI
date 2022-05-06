using System;
using System.Collections.Generic;
using UnityEngine;

namespace XTown.UI {
    /** A group can contain childrens of elements. THis class exists for my sanity. 
     */
    public class Group : Element {
        public List<Element> children = new List<Element>();

        public override void UpdateBase() {
            base.UpdateBase();

            children.ForEach(child => child.UpdateBase());
        }

        public static new Group New() {
            return _newObject<Group>();
        }

        public virtual void AddChild(Element child) {
            child.rect.SetParent(rect, false);
            child.parent = this;
            children.Add(child);
            ChildrenChanged();
        }

        public virtual void AddChildAt(Element child, int index) {
            child.rect.SetParent(rect, false);
            child.parent = this;
            child.SetZIndex(index);
            children.Add(child);
            ChildrenChanged();
        }

        /** Removes the element. Do not remove a child element directly; instead call this. */
        public virtual void RemoveChild(Element child) {
            if (child.rect.IsChildOf(rect)) {
                children.Remove(child);
                child.Remove();
            }
            ChildrenChanged();
        }

        /** Returns a temporary array! Note: contains grandchildren. */
        public Element[] GetAllChildren() {
            return rect.GetComponentsInChildren<Element>();
        }

        public void ForEach(Action<Element> act) {
            Element[] children = GetAllChildren();
            foreach (Element child in children) act(child);
        }

        public override void SetScene(Canvas scene) {
            base.SetScene(scene);
            ForEach(e => e.scene = scene);
        }

        public virtual void ClearChildren() {
            int n = rect.childCount;
            if (n <= 0) return;
            for (int i = n - 1; i >= 0; i--) {
                Destroy(rect.GetChild(i).gameObject);
            }
            children.Clear();
            ChildrenChanged();
        }

        public virtual void ChildrenChanged() {

        }
    }
}