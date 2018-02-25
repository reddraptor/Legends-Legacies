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
    GameObject[] tilePrefabs;
    string[] tilePrefabNames;
    int prefabSelect;
    

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

        tilePrefabs = tileSet.terrainTilePrefabs;
        tilePrefabNames = tileSet.GetTilePrefabNames();

        GUILayout.Label(tileName, EditorStyles.boldLabel);
        prefabSelect = GUILayout.SelectionGrid(terrainTile.prefabIndex, tilePrefabNames, 1);

        if (prefabSelect != terrainTile.prefabIndex)
        {
            parentChunk.ReplaceTile(terrainTile.indices, prefabSelect);
        }

        tileName = EditorGUILayout.TextField("Text Field", tileName);
    }
}