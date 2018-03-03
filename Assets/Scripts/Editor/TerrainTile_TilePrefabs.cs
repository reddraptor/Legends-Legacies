using UnityEngine;
using UnityEditor;
using Assets.Scripts.Components;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Helpers;


public class TerrainTile_TilePrefabs : EditorWindow
{
    string tileName;
    TerrainTileComponent terrainTile;
    GameObject selection;
    ChunkComponent parentChunk;
    TileSet tileSet;
    int prefabSelect;
    Indices indices;
    GUIContent[] tileSetContents;


    [MenuItem("CONTEXT/TerrainTileComponent/Tile Prefabs")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TerrainTile_TilePrefabs));
    }


    private void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        selection = Selection.activeGameObject;
        if (selection) terrainTile = selection.GetComponent<TerrainTileComponent>();
        if (terrainTile) parentChunk = terrainTile.GetComponentInParent<ChunkComponent>();
        if (parentChunk) tileSet = parentChunk.TileSet;

        titleContent.text = "Tile Prefabs";

        if (selection && terrainTile && parentChunk && tileSet)
        {
            tileSetContents = new GUIContent[tileSet.terrainTilePrefabs.Length];
            for (int i = 0; i < tileSet.terrainTilePrefabs.Length; i++)
            {
                Texture2D thumbnail = AssetPreview.GetAssetPreview(tileSet.terrainTilePrefabs[i]);
                tileSetContents[i] = new GUIContent(tileSet.terrainTilePrefabs[i].name, thumbnail);
            }
            
            tileName = terrainTile.name;
        }
        else
        {
            tileName = "No selection";
        }


        GUILayout.Label(tileName, EditorStyles.boldLabel);

        if (selection && terrainTile && parentChunk && tileSet)
        {
            GUILayout.Label("Tile Set: " + tileSet.name);

            prefabSelect = GUILayout.SelectionGrid(terrainTile.prefabIndex, tileSetContents, 1);
            if (prefabSelect != terrainTile.prefabIndex)
            {
                indices = terrainTile.indices;
                parentChunk.ReplaceTile(indices, prefabSelect);
                Selection.activeGameObject = parentChunk.GetTerrainTileAt(indices).gameObject;
            }
        }
    }
}