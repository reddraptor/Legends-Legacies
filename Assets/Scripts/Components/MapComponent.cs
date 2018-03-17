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
        public ViewportComponent viewPort;
        public ChunkComponent defaultChunk;
        public int chunkSize;
        public bool chunkLoadingEnabled = true;
        public Vector2 worldPosition;
        public LocationData.Coordinates centerCoordinates;
        public AreaLoaderComponent loadedArea;
        
        // Use this for initialization
        void Start()
        {
            transform.position = Vector2.zero;
            loadedArea = GetComponentInChildren<AreaLoaderComponent>();
            centerCoordinates = new LocationData.Coordinates(0, 0, chunkSize / 2, chunkSize / 2);
        }

        // Update is called once per frame
        void Update()
        {
            worldPosition = transform.position;
            
            if (viewPort.location.mapIndex == worldMapIndex)
            {
                loadedArea.transform.position = viewPort.transform.position;

                if (chunkLoadingEnabled)
                {
                    loadedArea.reload = true;
                }
            }
        }
    }

}