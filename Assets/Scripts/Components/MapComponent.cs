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
        public ChunkComponent defaultChunk;
        public int chunkSize;
        public Vector2 worldPosition;
        public LocationData.Coordinates centerCoordinates;
        
        // Use this for initialization
        void Start()
        {
            transform.position = Vector2.zero;
            centerCoordinates = new LocationData.Coordinates(0, 0, chunkSize / 2, chunkSize / 2);
        }

        // Update is called once per frame
        void Update()
        {
            worldPosition = transform.position;
        }
    }

}