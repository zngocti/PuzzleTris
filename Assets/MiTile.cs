using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MiTile : Tile
{
    public void nombre()
    {
        Debug.Log("sadf");
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/Mi Tile")]
    public static void CreateMiTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Mi Tile", "New Mi Tile", "Asset", "Save Mi Tile", "Assets");
        if (path == "") return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<MiTile>(), path);
    }
#endif

}