using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuItems
{
    [MenuItem("Tools/Print Icon PNG", false, 100)]
    public static void PrintIcon() {
        Debug.Log(Selection.activeObject.name);
        string spriteName = Selection.activeObject.name;

        string path = "tools/" + spriteName + ".png";
        //Texture2D texf = AssetPreview.GetMiniTypeThumbnail(typeof(UnityEngine.UI.GridLayoutGroup));
        Texture2D texf = (Texture2D)EditorGUIUtility.ObjectContent(Selection.activeObject, Selection.activeObject.GetType()).image;
        Texture2D tex = new Texture2D(texf.width, texf.height);
        Debug.Log(texf.width);
        Graphics.CopyTexture(texf, tex);
        if (tex == null) {
            Debug.LogError("Error: Failed to print " + spriteName);
            return;
        }
        byte[] bytes = tex.EncodeToPNG();
        if (bytes == null) {
            Debug.LogError("Error: Failed to export " + spriteName);
            return;
        }

        System.IO.File.WriteAllBytes(path, bytes);
    }

    [MenuItem("Tools/Print Icon PNG", true)]
    private static bool PrintIconValid() {
        return Selection.activeObject != null;
    }
}
