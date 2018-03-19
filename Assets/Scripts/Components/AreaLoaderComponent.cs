using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using Assets.Scripts.EditorAttributes;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(LocationComponent), typeof(ViewportComponent))]
    public class AreaLoaderComponent : MonoBehaviour
    {
        public int range = 3;
        [ReadOnly] public int width;
        public LocationComponent location;
        public ViewportComponent viewport;
        public Vector2 worldPosition;
        public bool chunkLoadingEnabled = true;
        public bool reload = true;

        private ChunkComponent[,] chunks;

        void LoadArea()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (!chunks[i, j]) chunks[i,j] = LoadChunk(new Indices(i, j));
                }
            }
        }

        ChunkComponent LoadChunk(Indices chunkIndices)
        {
            ChunkComponent chunk = location.map.defaultChunk;

            chunk = Instantiate(chunk,
                new Vector3(
                    worldPosition.x + (-range + chunkIndices.i) * location.map.chunkSize,
                    worldPosition.y + (-range + chunkIndices.j) * location.map.chunkSize
                    ),
                Quaternion.identity, location.map.transform);
             
            chunk.loadTilesOutsideViewport = false;
            chunk.viewport = viewport;
            chunk.areaLoader = this;
            chunk.mapLoadAreaIndices = chunkIndices;
            chunk.name = chunk.name + "[" + chunkIndices.i + ", " + chunkIndices.j + "]";

            return chunk;
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
            location = GetComponent<LocationComponent>();
            viewport = GetComponent<ViewportComponent>();
            worldPosition = transform.position;
            width = range * 2 + 1;
            chunks = new ChunkComponent[width, width];
        }

        // Update is called once per frame
        void Update()
        {
            worldPosition = transform.position;
            if (location.map & chunkLoadingEnabled & reload == true)
            {
                LoadArea();
                reload = false;
            }
        }
    }

}