using UnityEngine;
using UnityEditor;
using Assets.Scripts.Components;
using Assets.Scripts.ScriptableObjects;


public class TerrainTile_TilePrefabs : EditorWindow
{
    string tileName;
    TerrainTile terrainTile;
    GameObject selection;
    Chunk parentChunk;
    TileSet tileSet;
    

    [MenuItem("CONTEXT/TerrainTile/Tile Prefabs")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TerrainTile_TilePrefabs));
    }

    void OnGUI()
    {
        selection = Selection.activeGameObject;
        if (selection) terrainTile = selection.GetComponent<TerrainTile>();
        if (terrainTile) parentChunk = terrainTile.GetComponentInParent<Chunk>();
        if (parentChunk) tileSet = parentChunk.GetTileSet();

        titleContent.text = "Tile Prefabs";

        if (selection && terrainTile)
        {
            tileName = terrainTile.name;
        }
        else
        {
            tileName = "No selection";
        }

        GUILayout.Label(tileName, EditorStyles.boldLabel);
        //GUILayout.SelectionGrid(terrainTile.prefabIndex, )

        tileName = EditorGUILayout.TextField("Text Field", tileName);

    }
}