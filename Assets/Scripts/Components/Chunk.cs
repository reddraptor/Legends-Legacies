using System;
using UnityEngine;
using Assets.Scripts.EditorAttributes;
using Assets.Scripts.Serialization;
using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Components
{
    public class Chunk : MonoBehaviour, IHasSerializableData<ChunkData>
    {

        public PrefabTables prefabTables;
        public int tileSetIndex;
        public int size = 32;
        public bool procedualGeneration = false;
        
        private TerrainTile[,] terrainTiles;

        public TerrainTile GetTerrainTileAt(TerrainTile.Indices tileIndices)
        {
            if (terrainTiles == null) Start();
            return terrainTiles[tileIndices.i, tileIndices.j];
        }

        public TileSet GetTileSet()
        {
            return prefabTables.tileSetTable[tileSetIndex];
        }

        public Vector3 CalculatedPositionForTileAt(TerrainTile.Indices tileIndices)
        {
            return new Vector3(transform.position.x - (terrainTiles.GetLength(0) / 2) + tileIndices.i, transform.position.y - (terrainTiles.GetLength(1) / 2) + tileIndices.j);
        }

        public TerrainTile ReplaceTile(TerrainTile.Indices tileIndices, int prefabIndex)
        {
            if (terrainTiles == null) Start();

            return InstantiateTile(tileIndices, prefabIndex);
        }

        public void SetTileSet(int newTileSetIndex)
        {
            tileSetIndex = newTileSetIndex;
            Refresh();
        }

        private void Refresh()
        {
            if (terrainTiles == null)
                Initialize();
            else
            {
                for (int i = 0; i < terrainTiles.GetLength(0); i++)
                {
                    for (int j = 0; j < terrainTiles.GetLength(1); j++)
                    {
                        InstantiateTile(new TerrainTile.Indices(i, j), terrainTiles[i, j].prefabIndex);
                    }
                }
            }
        }

        internal void ResetTilePosition(TerrainTile.Indices tileIndices)
        {
            terrainTiles[tileIndices.i, tileIndices.j].transform.position = CalculatedPositionForTileAt(tileIndices);
        }

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            terrainTiles = new TerrainTile[size, size];
            TerrainTile[] terrainTileList = GetComponentsInChildren<TerrainTile>();

            foreach (TerrainTile terrainTile in terrainTileList)
            {
                terrainTiles[terrainTile.indices.i, terrainTile.indices.j] = terrainTile;
            }

            Refresh();
        }

        // Update is called once per frame
        void Update()
        {
            // Ensure terrain tile indices property hasn't been modified, and revert if necessary to reflect it's location in the chunk
            for (int i = 0; i < terrainTiles.GetLength(0); i++)
            {
                for (int j = 0; j < terrainTiles.GetLength(1); j++)
                {
                    TerrainTile terrainTile = terrainTiles[i, j];

                    if (terrainTile.indices.i != i) terrainTile.indices.i = i;
                    if (terrainTile.indices.j != j) terrainTile.indices.j = j;
                    if (terrainTile.transform.position != CalculatedPositionForTileAt(terrainTile.indices)) ResetTilePosition(terrainTile.indices);
                }
            }
        }

        private TerrainTile InstantiateTile(TerrainTile.Indices tileIndices, int terrainTilePrefabIndex)
        {
            if (tileIndices.i < terrainTiles.GetLength(0) && tileIndices.i >= 0 &&
                tileIndices.j < terrainTiles.GetLength(1) && tileIndices.j >= 0 &&
                terrainTilePrefabIndex < GetTileSet().terrainTilePrefabs.Length && terrainTilePrefabIndex >= 0)
            {
                GameObject tileObject = Instantiate(GetTileSet().terrainTilePrefabs[terrainTilePrefabIndex], CalculatedPositionForTileAt(tileIndices), Quaternion.identity, transform);

                TerrainTile terrainTileComponent;

                if (terrainTileComponent = tileObject.GetComponent<TerrainTile>())
                {
                    if (terrainTiles[tileIndices.i, tileIndices.j])
                    {
#if UNITY_EDITOR
                        DestroyImmediate(terrainTiles[tileIndices.i, tileIndices.j].gameObject);
#else
                        Destroy(terrainTiles[chunkIndices.i, chunkIndices.j].gameObject);
#endif
                    }
                    terrainTiles[tileIndices.i, tileIndices.j] = terrainTileComponent;
                    terrainTiles[tileIndices.i, tileIndices.j].indices = tileIndices;
                    terrainTiles[tileIndices.i, tileIndices.j].prefabIndex = terrainTilePrefabIndex;
                }
                else
                {
                    Debug.Log("InstantiateTile(chunkIndices, prefabIndex); " + tileObject + " has no TerrainTile component.");
                    Destroy(tileObject);
                    return null;
                }

                return terrainTileComponent;
            }
            else
            {
                Debug.Log("InstantiateTile(chunkIndices, prefabIndex) arguments out of range.");
                return null;
            }
        }

        public ChunkData GetSerializableData()
        {
            ChunkData data = new ChunkData(tileSetIndex, size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    data.tilePrefabIndex[i, j] = terrainTiles[i, j].prefabIndex;
                }
            }
            return data;
        }

        public void SetFromSerializableData(ChunkData data)
        {
            size = data.size;
            tileSetIndex = data.tileSetIndex;

            for (int i = 0; i < terrainTiles.GetLength(0); i++)
            {
                for (int j = 0; j < terrainTiles.GetLength(1); j++)
                {
                    InstantiateTile(new TerrainTile.Indices(i, j), data.tilePrefabIndex[i, j]);
                    terrainTiles[i, j].prefabIndex = data.tilePrefabIndex[i,j];
                }
            }

        }

        [ContextMenu("Refresh")]
        private void RefreshFromMenu()
        {
            Refresh();
        }
    }

}