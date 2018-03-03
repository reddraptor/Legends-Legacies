using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.EditorAttributes;
using Assets.Scripts.Serialization;
using Assets.Scripts.Helpers;
using System;

namespace Assets.Scripts.Components
{
    public class MapComponent : MonoBehaviour
    {
        [ReadOnly] public int worldMapIndex = 0;
        public ViewPortComponent viewPort;
        public ChunkComponent defaultChunk;
        public int chunkSize;
        public bool chunkLoadingEnabled = true;
        public Vector2 worldPosition;
        public LocationData.Coordinates centerCoordinates;
        public LoadedArea loadedArea;
        
        [Serializable]
        public class LoadedArea
        {
            public ChunkComponent[,] chunks;
         
            public int range = 3;
            [ReadOnly]public int width;
            [ReadOnly]public int chunkSize;
            public LocationData.Coordinates centerCoordinates;

            public LoadedArea(int chunkSize)
            {
                width = range * 2 + 1;
                chunks = new ChunkComponent[width, width];
                centerCoordinates = new LocationData.Coordinates(0, 0, chunkSize / 2, chunkSize / 2);
            }
        }

        // Use this for initialization
        void Start()
        {
            transform.position = Vector2.zero;
            loadedArea = new LoadedArea(chunkSize);
            centerCoordinates = new LocationData.Coordinates(0, 0, chunkSize / 2, chunkSize / 2);
        }

        // Update is called once per frame
        void Update()
        {
            worldPosition = transform.position;

            if (viewPort.location.mapIndex == worldMapIndex)
            {
                loadedArea.centerCoordinates.chunk = viewPort.location.coordinates.chunk;

                if (chunkLoadingEnabled)
                {
                    for (int i = 0; i < loadedArea.width; i++)
                    {
                        for (int j = 0; j < loadedArea.width; j++)
                        {
                            if (!loadedArea.chunks[i, j])
                                LoadChunk(new Indices(i, j));
                        }
                    }
                }
            }
        }

        void DestroyChunk(ChunkComponent chunk)
        {
#if UNITY_EDITOR
            DestroyImmediate(chunk);
#else
            Destroy(chunk);
#endif
        }

        ChunkComponent LoadChunk(Indices areaLoadIndices)
        {
            ChunkComponent chunkComponent;

            chunkComponent = Instantiate(defaultChunk,
                new Vector3(
                    worldPosition.x - loadedArea.range + (areaLoadIndices.i * chunkSize),
                    worldPosition.y - loadedArea.range + (areaLoadIndices.j * chunkSize)
                    ),
                Quaternion.identity, transform);

            chunkComponent.loadTilesOutsideViewport = false;
            chunkComponent.viewPort = viewPort;
            chunkComponent.mapLoadAreaIndices = areaLoadIndices;
            chunkComponent.name = chunkComponent.name + "[" + areaLoadIndices.i + ", " + areaLoadIndices.j + "]";
            loadedArea.chunks[areaLoadIndices.i, areaLoadIndices.j] = chunkComponent;

            return chunkComponent;
        }
    }

}