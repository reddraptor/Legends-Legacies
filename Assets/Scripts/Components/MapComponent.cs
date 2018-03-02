using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.EditorAttributes;
using Assets.Scripts.Serialization;

namespace Assets.Scripts.Components
{
    public class MapComponent : MonoBehaviour
    {
        public ViewPortComponent viewPort;
        public ChunkComponent defaultChunk;
        public int loadRange = 3;
        [ReadOnly]public int index = 0;
        [ReadOnly]public int chunkSize;
        
        ChunkComponent[,] loadedChunks;
        int loadAreaWidth;
        
        // Use this for initialization
        void Start()
        {
            transform.position = Vector2.zero;
            chunkSize = defaultChunk.size;
            loadAreaWidth = loadRange * 2 + 1;
            loadedChunks = new ChunkComponent[loadAreaWidth, loadAreaWidth];

            ChunkComponent chunkComponent;
            Vector3 positionModifier;

            for (int i = 0; i < loadedChunks.GetLength(0); i++)
            {
                for (int j = 0; j < loadedChunks.GetLength(1); j++)
                {
                    positionModifier = new Vector3(chunkSize * (-loadRange + i), chunkSize * (-loadRange + j));
                    chunkComponent = Instantiate(defaultChunk, transform.position + positionModifier, Quaternion.identity, transform);
                    chunkComponent.loadTilesOutsideViewport = false;
                    chunkComponent.viewPort = viewPort;
                    loadedChunks[i, j] = chunkComponent;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }


    }

}