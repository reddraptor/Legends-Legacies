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
    int prefabSelect;
    TerrainTile.Indices indices;
    GUIContent[] tileSetContents;
    

    [MenuItem("CONTEXT/TerrainTile/Tile Prefabs")]
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
        if (selection) terrainTile = selection.GetComponent<TerrainTile>();
        if (terrainTile) parentChunk = terrainTile.GetComponentInParent<Chunk>();
        if (parentChunk) tileSet = parentChunk.GetTileSet();

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