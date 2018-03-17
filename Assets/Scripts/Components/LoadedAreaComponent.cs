using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using Assets.Scripts.EditorAttributes;

namespace Assets.Scripts.Components
{
    public class LoadedAreaComponent : MonoBehaviour
    {
        public int range = 3;
        [ReadOnly] public int width;
        public MapComponent map;
        public LocationComponent location;
        public Vector2 worldPosition;
        public bool reload = false;

        private ChunkComponent[,] chunks;

        void Load()
        {
            ChunkComponent chunk;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (!(chunk = chunks[i, j])) LoadChunk(new Indices(i, j));
                }
            }
        }

        ChunkComponent LoadChunk(Indices chunkIndices)
        {
            ChunkComponent chunkComponent = map.defaultChunk;

            chunkComponent = Instantiate(chunkComponent,
                new Vector3(
                    worldPosition.x + (-range + chunkIndices.i) * map.chunkSize,
                    worldPosition.y + (-range + chunkIndices.j) * map.chunkSize
                    ),
                Quaternion.identity, map.transform);

            chunkComponent.loadTilesOutsideViewport = false;
            chunkComponent.viewPort = map.viewPort;
            chunkComponent.mapLoadAreaIndices = chunkIndices;
            chunkComponent.name = chunkComponent.name + "[" + chunkIndices.i + ", " + chunkIndices.j + "]";
            chunks[chunkIndices.i, chunkIndices.j] = chunkComponent;

            return chunkComponent;
        }

        void DestroyChunk(ChunkComponent chunk)
        {
#if UNITY_EDITOR
            DestroyImmediate(chunk);
#else
            Destroy(chunk);
#endif
        }

        // Use this for initialization
        void Start()
        {
            map = GetComponentInParent<MapComponent>();
            location = GetComponent<LocationComponent>();
            location.map = map;
            location.mapIndex = map.worldMapIndex;
            width = range * 2 + 1;
            chunks = new ChunkComponent[width, width];
        }

        // Update is called once per frame
        void Update()
        {
            worldPosition = transform.position;
            if (reload)
            {
                Load();
                reload = false;
            }
        }
    }

}