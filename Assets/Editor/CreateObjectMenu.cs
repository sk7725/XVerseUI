using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace XVerse.UI {
    public static class CreateObjectMenu {
        private const float width = 160f;
        private const float thickHeight = 30f;
        private const float thinHeight = 20f;

        [MenuItem("GameObject/XUI/XButton", false, 10)]
        public static void AddButton(MenuCommand menuCommand) {
            XButton go = XButton.New("Button", () => { });
            go.Get().Size(width, thickHeight);

            // Override font size
            Label textComponent = go.GetComponentInChildren<Label>();

            PlaceUIElementRoot(go.gameObject, menuCommand);
        }

        [MenuItem("GameObject/XUI/XImage", false, 11)]
        public static void AddImage(MenuCommand menuCommand) {
            XImage go = XImage.New();
            PlaceUIElementRoot(go.gameObject, menuCommand);
        }

        [MenuItem("GameObject/XUI/XRawImage", false, 12)]
        public static void AddRawImage(MenuCommand menuCommand) {
            XRawImage go = XRawImage.New();
            PlaceUIElementRoot(go.gameObject, menuCommand);
        }

        [MenuItem("GameObject/XUI/Background", false, 13)]
        public static void AddBackground(MenuCommand menuCommand) {
            Background go = Background.New();
            PlaceUIElementRoot(go.gameObject, menuCommand);
        }

        [MenuItem("GameObject/XUI/Label", false, 14)]
        public static void AddLabel(MenuCommand menuCommand) {
            Label go = Label.New("New Text");
            go.gameObject.name = "Label";
            go.Get().Size(200, 50);
            go.fontSize = 36;

            PlaceUIElementRoot(go.gameObject, menuCommand);
        }

        // Code provided by TextMeshPro package of Unity
        private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand) {
            GameObject parent = menuCommand.context as GameObject;
            bool explicitParentChoice = true;
            if (parent == null) {
                parent = TMPro_CreateObjectMenu.GetOrCreateCanvasGameObject();
                explicitParentChoice = false;

                // If in Prefab Mode, Canvas has to be part of Prefab contents,
                // otherwise use Prefab root instead.
                PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null && !prefabStage.IsPartOfPrefabContents(parent))
                    parent = prefabStage.prefabContentsRoot;
            }

            if (parent.GetComponentsInParent<Canvas>(true).Length == 0) {
                // Create canvas under context GameObject,
                // and make that be the parent which UI element is added under.
                GameObject canvas = TMPro_CreateObjectMenu.CreateNewUI();
                Undo.SetTransformParent(canvas.transform, parent.transform, "");
                parent = canvas;
            }

            GameObjectUtility.EnsureUniqueNameForSibling(element);

            SetParentAndAlign(element, parent);
            if (!explicitParentChoice) // not a context click, so center in sceneview
                SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

            // This call ensure any change made to created Objects after they where registered will be part of the Undo.
            Undo.RegisterFullObjectHierarchyUndo(parent == null ? element : parent, "");

            // We have to fix up the undo name since the name of the object was only known after reparenting it.
            Undo.SetCurrentGroupName("Create " + element.name);

            Selection.activeGameObject = element;
        }

        private static void SetParentAndAlign(GameObject child, GameObject parent) {
            if (parent == null)
                return;

            Undo.SetTransformParent(child.transform, parent.transform, "");

            RectTransform rectTransform = child.transform as RectTransform;
            if (rectTransform) {
                rectTransform.anchoredPosition = Vector2.zero;
                Vector3 localPosition = rectTransform.localPosition;
                localPosition.z = 0;
                rectTransform.localPosition = localPosition;
            }
            else {
                child.transform.localPosition = Vector3.zero;
            }
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;

            SetLayerRecursively(child, parent.layer);
        }

        private static void SetLayerRecursively(GameObject go, int layer) {
            go.layer = layer;
            Transform t = go.transform;
            for (int i = 0; i < t.childCount; i++)
                SetLayerRecursively(t.GetChild(i).gameObject, layer);
        }

        private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform) {
            // Find the best scene view
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView == null && SceneView.sceneViews.Count > 0)
                sceneView = SceneView.sceneViews[0] as SceneView;

            // Couldn't find a SceneView. Don't set position.
            if (sceneView == null || sceneView.camera == null)
                return;

            // Create world space Plane from canvas position.
            Camera camera = sceneView.camera;
            Vector3 position = Vector3.zero;
            Vector2 localPlanePosition;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition)) {
                // Adjust for canvas pivot
                localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
                localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

                localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
                localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

                // Adjust for anchoring
                position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
                position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

                Vector3 minLocalPosition;
                minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
                minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

                Vector3 maxLocalPosition;
                maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
                maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

                position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
                position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
            }

            itemTransform.anchoredPosition = position;
            itemTransform.localRotation = Quaternion.identity;
            itemTransform.localScale = Vector3.one;
        }
    }
}