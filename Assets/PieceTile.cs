using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PieceTile : Tile
{
    public void nombre()
    {
        Debug.Log("sadf");
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Custom Tiles/Piece Tile")]
    public static void CreatePieceTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Piece Tile", "New Piece Tile", "Asset", "Save Piece Tile", "Assets");
        if (path == "") return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PieceTile>(), path);
    }
#endif

}