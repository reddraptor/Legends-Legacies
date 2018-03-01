using UnityEngine;
using Assets.Scripts.Serialization;
using System;

namespace Assets.Scripts.Components
{
    public class Location : MonoBehaviour, IHasSerializableData<LocationData>
    {
        public int mapIndex;
        public LocationData.Coordinates coordinates;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public LocationData GetSerializableData()
        {
            return new LocationData(mapIndex, coordinates);
        }

        public void SetFromSerializableData(LocationData data)
        {
            mapIndex = data.mapIndex;
            coordinates = data.coordinates;
        }
    }

}