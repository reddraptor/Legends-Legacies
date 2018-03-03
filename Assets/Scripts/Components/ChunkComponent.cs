using System;
using UnityEngine;
using Assets.Scripts.EditorAttributes;
using Assets.Scripts.Serialization;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Helpers;

namespace Assets.Scripts.Components
{
    public class ChunkComponent : MonoBehaviour, IHasSerializableData<ChunkData>
    {
        public ViewPortComponent viewPort;
        public PrefabTables prefabTables;
        public int prefabTableTileSetIndex = 0;
        public TileSet tileSet;
        public int size = 32;
        public bool proceduralGeneration = false;
        public bool loadTilesOutsideViewport = false;
        [ReadOnly] public Indices mapLoadAreaIndices;
        [ReadOnly] public Vector2 worldPosition;
        [ReadOnly] public Vector2 lowerLeftCorner;
        [ReadOnly] public Vector2 upperRightCorner;
        
        private TerrainTileComponent[,] terrainTileComponents;
        private TerrainTileData[,] terrainTileData;

        public TerrainTileComponent GetTerrainTileAt(Indices tileIndices)
        {
            if (terrainTileComponents == null) Start();
            return terrainTileComponents[tileIndices.i, tileIndices.j];
        }

        /// <summary>
        /// Returns the tileset this chunk will use. If no specific tile set is allocated, the tile set
        /// from the allocated prefab table and table index will be used. If no prefab table is allocated, null is returned.
        /// </summary>
        public TileSet TileSet
        {
            get
            {
                if (tileSet)
                    return tileSet;
                else if (prefabTables)
                    return prefabTables.tileSetTable[prefabTableTileSetIndex];
                else
                    return null;
            }
            set
            {
                tileSet = value;
            }
        }

        public Vector3 GetPositionAtIndices(Indices tileIndices)
        {
            return new Vector3(worldPosition.x - (size / 2) + tileIndices.i, worldPosition.y - (size / 2) + tileIndices.j);
        }

        public TerrainTileComponent ReplaceTile(Indices tileIndices, int prefabIndex)
        {
            terrainTileData[tileIndices.i, tileIndices.j].prefabIndex = prefabIndex;
            
            if (terrainTileComponents == null) Start();

            if (loadTilesOutsideViewport || InViewPort(tileIndices))
                return LoadTerrainTile(tileIndices, prefabIndex);
            else
                return null;
        }

        private void Reload()
        {
            if (terrainTileComponents == null)
                Initialize();
            else
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (loadTilesOutsideViewport || InViewPort(new Indices(i, j)))
                            LoadTerrainTile(new Indices(i, j), terrainTileData[i, j].prefabIndex);
                    }
                }
            }
        }

        internal void ResetTilePosition(Indices tileIndices)
        {
            terrainTileComponents[tileIndices.i, tileIndices.j].transform.position = GetPositionAtIndices(tileIndices);
        }

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            TileSet = prefabTables.tileSetTable[prefabTableTileSetIndex];

            terrainTileData = new TerrainTileData[size, size];
            for (int i=0; i < size; i++)
            {
                for (int j=0; j <size; j++)
                {
                    terrainTileData[i,j] = TileSet.terrainTilePrefabs[TileSet.defaultIndex].GetComponent<TerrainTileComponent>().GetSerializableData();
                }
            }

            terrainTileComponents = new TerrainTileComponent[size, size];
            TerrainTileComponent[] terrainTileList = GetComponentsInChildren<TerrainTileComponent>();

            foreach (TerrainTileComponent terrainTileComponent in terrainTileList)
            {
                terrainTileComponents[terrainTileComponent.indices.i, terrainTileComponent.indices.j] = terrainTileComponent;
                terrainTileData[terrainTileComponent.indices.i, terrainTileComponent.indices.j] = terrainTileComponent.GetSerializableData();
            }

            Reload();
        }

        // Update is called once per frame
        void Update()
        {
            worldPosition = transform.position;
            lowerLeftCorner.x = worldPosition.x - (size / 2);
            lowerLeftCorner.y = worldPosition.y - (size / 2);
            upperRightCorner.x = lowerLeftCorner.x + size;
            upperRightCorner.x = upperRightCorner.y + size;

            for (int i = 0; i < terrainTileComponents.GetLength(0); i++)
            {
                for (int j = 0; j < terrainTileComponents.GetLength(1); j++)
                {
                    TerrainTileComponent terrainTile = terrainTileComponents[i, j];

                    if (terrainTile)
                    {
                        // Ensure terrain tile indices property hasn't been modified, and revert if necessary to reflect it's location in the chunk

                        if (terrainTile.indices.i != i) terrainTile.indices.i = i;
                        if (terrainTile.indices.j != j) terrainTile.indices.j = j;
                        if (terrainTile.transform.position != GetPositionAtIndices(terrainTile.indices)) ResetTilePosition(terrainTile.indices);

                        // If we don't want have loaded tile outside of viewport, unload the tile gameobject if necessary
                        if (!loadTilesOutsideViewport && !InViewPort(new Indices(i, j)))
                        {
                            UnloadTerrainTile(terrainTile);
                        }
                    }
                    else
                    {
                        if (loadTilesOutsideViewport || InViewPort(new Indices(i, j)))
                        {
                            LoadTerrainTile(new Indices(i, j), terrainTileData[i,j].prefabIndex);
                        }
                    }
                }
            }
        }

        private TerrainTileComponent LoadTerrainTile(Indices tileIndices, int terrainTilePrefabIndex)
        {
            if (tileIndices.i < terrainTileComponents.GetLength(0) && tileIndices.i >= 0 &&
                tileIndices.j < terrainTileComponents.GetLength(1) && tileIndices.j >= 0 &&
                terrainTilePrefabIndex < TileSet.terrainTilePrefabs.Length && terrainTilePrefabIndex >= 0)
            {
                GameObject tileObject = Instantiate(TileSet.terrainTilePrefabs[terrainTilePrefabIndex], GetPositionAtIndices(tileIndices), Quaternion.identity, transform);

                TerrainTileComponent terrainTile;

                if (terrainTile = tileObject.GetComponent<TerrainTileComponent>())
                {
                    if (terrainTileComponents[tileIndices.i, tileIndices.j])
                    {
                        UnloadTerrainTile(terrainTileComponents[tileIndices.i, tileIndices.j]);
                    }
                    terrainTileComponents[tileIndices.i, tileIndices.j] = terrainTile;
                    terrainTileComponents[tileIndices.i, tileIndices.j].indices = tileIndices;
                    terrainTileComponents[tileIndices.i, tileIndices.j].prefabIndex = terrainTilePrefabIndex;
                    terrainTileData[tileIndices.i, tileIndices.j] = terrainTile.GetSerializableData();
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
            ChunkData data = new ChunkData(prefabTableTileSetIndex, size)
            {
                terrainTileData = terrainTileData
            };
            return data;
        }

        public void SetFromSerializableData(ChunkData data)
        {
            size = data.size;
            prefabTableTileSetIndex = data.tileSetIndex;
            TileSet = prefabTables.tileSetTable[prefabTableTileSetIndex];
            terrainTileData = data.terrainTileData;

            Reload();
        }

        public bool InViewPort(Indices indices)
        {
            if (indices.i < 0 || indices.j < 0 || indices.i >= size || indices.j >= size) return false;

            return viewPort.InView(GetPositionAtIndices(indices));
        }

        public void UnloadTerrainTile(TerrainTileComponent terrainTile)
        {
            #if UNITY_EDITOR
                        DestroyImmediate(terrainTile.gameObject);
            #else
                        Destroy(terrainTile.gameObject);
            #endif
        }

        [ContextMenu("Reload Chunk")]
        private void ReloadFromMenu()
        {
            Reload();
        }
    }

}