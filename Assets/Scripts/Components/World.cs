using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Serialization;
using System;

namespace Assets.Scripts.Components
{
    public class World : MonoBehaviour, IHasSerializableData<WorldData>
    {
        public int chunkSize = 32;

        private Dictionary<LocationData.Coordinates, Chunk> savedChunks;

        public WorldData GetSerializableData()
        {
            throw new NotImplementedException();
        }

        public void SetFromSerializableData(WorldData data)
        {
            throw new NotImplementedException();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}