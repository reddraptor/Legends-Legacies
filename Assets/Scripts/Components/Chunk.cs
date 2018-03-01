using System;
using UnityEngine;
using Assets.Scripts.EditorAttributes;
using Assets.Scripts.Serialization;
using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Components
{
    public class Chunk : MonoBehaviour, IHasSerializableData<ChunkData>
    {
        public new Camera camera;
        public PrefabTables prefabTables;
        public int tileSetIndex;
        public int size = 32;
        public bool procedualGeneration = false;
        public bool loadTilesOutsideViewport = false;
        
        private TerrainTile[,] terrainTiles;
        private int[,] tilePrefabIndex;

        public TerrainTile GetTerrainTileAt(TerrainTile.Indices tileIndices)
        {
            if (terrainTiles == null) Start();
            return terrainTiles[tileIndices.i, tileIndices.j];
        }

        public TileSet GetTileSet()
        {
            return prefabTables.tileSetTable[tileSetIndex];
        }

        public Vector3 GetPositionAtIndices(TerrainTile.Indices tileIndices)
        {
            return new Vector3(transform.position.x - (terrainTiles.GetLength(0) / 2) + tileIndices.i, transform.position.y - (terrainTiles.GetLength(1) / 2) + tileIndices.j);
        }

        public TerrainTile ReplaceTile(TerrainTile.Indices tileIndices, int prefabIndex)
        {
            tilePrefabIndex[tileIndices.i, tileIndices.j] = prefabIndex;

            if (terrainTiles == null) Start();

            if (loadTilesOutsideViewport || InViewPort(tileIndices))
                return InstantiateTile(tileIndices, prefabIndex);
            else
                return null;
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
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (loadTilesOutsideViewport || InViewPort(new TerrainTile.Indices(i, j)))
                            InstantiateTile(new TerrainTile.Indices(i, j), tilePrefabIndex[i, j]);
                    }
                }
            }
        }

        internal void ResetTilePosition(TerrainTile.Indices tileIndices)
        {
            terrainTiles[tileIndices.i, tileIndices.j].transform.position = GetPositionAtIndices(tileIndices);
        }

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            tilePrefabIndex = new int[size, size];
            for (int i=0; i < size; i++)
            {
                for (int j=0; j <size; j++)
                {
                    tilePrefabIndex[i, j] = 0;
                }
            }

            terrainTiles = new TerrainTile[size, size];
            TerrainTile[] terrainTileList = GetComponentsInChildren<TerrainTile>();

            foreach (TerrainTile terrainTile in terrainTileList)
            {
                terrainTiles[terrainTile.indices.i, terrainTile.indices.j] = terrainTile;
                tilePrefabIndex[terrainTile.indices.i, terrainTile.indices.j] = terrainTile.prefabIndex;
            }

            Refresh();
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < terrainTiles.GetLength(0); i++)
            {
                for (int j = 0; j < terrainTiles.GetLength(1); j++)
                {
                    TerrainTile terrainTile = terrainTiles[i, j];

                    if (terrainTile)
                    {
                        // Ensure terrain tile indices property hasn't been modified, and revert if necessary to reflect it's location in the chunk

                        if (terrainTile.indices.i != i) terrainTile.indices.i = i;
                        if (terrainTile.indices.j != j) terrainTile.indices.j = j;
                        if (terrainTile.transform.position != GetPositionAtIndices(terrainTile.indices)) ResetTilePosition(terrainTile.indices);

                        // If we don't want have loaded tile outside of viewport, unload the tile gameobject if necessary
                        if (!loadTilesOutsideViewport && !InViewPort(new TerrainTile.Indices(i, j)))
                        {
                            DestroyTile(terrainTile);
                        }
                    }
                    else
                    {
                        if (loadTilesOutsideViewport || InViewPort(new TerrainTile.Indices(i, j)))
                        {
                            InstantiateTile(new TerrainTile.Indices(i, j), tilePrefabIndex[i, j]);
                        }
                    }
                }
            }
        }

        private TerrainTile InstantiateTile(TerrainTile.Indices tileIndices, int terrainTilePrefabIndex)
        {
            if (tileIndices.i < terrainTiles.GetLength(0) && tileIndices.i >= 0 &&
                tileIndices.j < terrainTiles.GetLength(1) && tileIndices.j >= 0 &&
                terrainTilePrefabIndex < GetTileSet().terrainTilePrefabs.Length && terrainTilePrefabIndex >= 0)
            {
                GameObject tileObject = Instantiate(GetTileSet().terrainTilePrefabs[terrainTilePrefabIndex], GetPositionAtIndices(tileIndices), Quaternion.identity, transform);

                TerrainTile terrainTile;

                if (terrainTile = tileObject.GetComponent<TerrainTile>())
                {
                    if (terrainTiles[tileIndices.i, tileIndices.j])
                    {
                        DestroyTile(terrainTiles[tileIndices.i, tileIndices.j]);
                    }
                    terrainTiles[tileIndices.i, tileIndices.j] = terrainTile;
                    terrainTiles[tileIndices.i, tileIndices.j].indices = tileIndices;
                    terrainTiles[tileIndices.i, tileIndices.j].prefabIndex = terrainTilePrefabIndex;
                    tilePrefabIndex[tileIndices.i, tileIndices.j] = terrainTilePrefabIndex;
                }
                else
                {
                    Debug.Log("InstantiateTile(chunkIndices, prefabIndex); " + tileObject + " has no TerrainTile component.");
                    Destroy(tileObject);
                    return null;
                }

                return terrainTile;
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
                    data.tilePrefabIndex[i, j] = tilePrefabIndex[i, j];
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
                    tilePrefabIndex[i,j] = data.tilePrefabIndex[i,j];
                }
            }

            Refresh();
        }

        public bool InViewPort(TerrainTile.Indices indices)
        {
            if (!camera) return false;

            if (indices.i < 0 || indices.j < 0 || indices.i >= size || indices.j >= size) return false;

            Vector2 lowerLeft = camera.ViewportToWorldPoint(new Vector3(0, 0));
            Vector2 upperRight = camera.ViewportToWorldPoint(new Vector3(1, 1));

            Vector2 tilePosition = GetPositionAtIndices(indices);

            if (tilePosition.x + 1 > lowerLeft.x && tilePosition.x - 1 < upperRight.x && tilePosition.y + 1 > lowerLeft.y && tilePosition.y - 1 < upperRight.y)
            {
                return true;
            }

            return false;
        }

        public void DestroyTile(TerrainTile terrainTile)
        {
            #if UNITY_EDITOR
                        DestroyImmediate(terrainTile.gameObject);
            #else
                        Destroy(terrainTile.gameObject);
            #endif
        }

        [ContextMenu("Refresh")]
        private void RefreshFromMenu()
        {
            Refresh();
        }
    }

}