using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.EditorAttributes;
using Assets.Scripts.Serialization;

namespace Assets.Scripts.Components
{
    public class Map : MonoBehaviour
    {
        public new Camera camera;
        public Chunk defaultChunk;
        public int loadRange = 1;
        [ReadOnly]public int index = 0;
        [ReadOnly]public int chunkSize;
        
        Chunk[,] loadedChunks;
        int loadAreaWidth;
        LocationData.Chunk LowerLeftChunkCoordinates;
        
        // Use this for initialization
        void Start()
        {
            transform.position = Vector2.zero;
            chunkSize = defaultChunk.size;
            loadAreaWidth = loadRange * 2 + 1;
            loadedChunks = new Chunk[loadAreaWidth, loadAreaWidth];
            LocationData.Coordinates cameraCoordinates = camera.GetComponent<Location>().coordinates;
            LowerLeftChunkCoordinates = new LocationData.Chunk(cameraCoordinates.chunk.x - loadRange, cameraCoordinates.chunk.y - loadRange);

            

            for (int i = 0; i < loadedChunks.GetLength(0); i++)
            {
                for (int j = 0; j < loadedChunks.GetLength(1); j++)
                {
                    Vector3 positionModifier = new Vector3(chunkSize * (-loadRange + i), chunkSize * (-loadRange + j));
                    loadedChunks[i, j] = Instantiate(defaultChunk, transform.position + positionModifier, Quaternion.identity, transform);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }


    }

}