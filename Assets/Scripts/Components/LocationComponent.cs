using UnityEngine;
using Assets.Scripts.Serialization;
using System;

namespace Assets.Scripts.Components
{
    public class LocationComponent : MonoBehaviour, IHasSerializableData<LocationData>
    {
        public int mapIndex;
        public MapComponent map;
        public LocationData.Coordinates coordinates;

        public LocationData GetSerializableData()
        {
            return new LocationData(mapIndex, coordinates);
        }

        public void SetFromSerializableData(LocationData data)
        {
            mapIndex = data.mapIndex;
            coordinates = data.coordinates;
        }

        public void SetFromWorldPosition(Vector2 position)
        {
            float positionDifferenceX = position.x - map.worldPosition.x;
            float positionDifferenceY = position.y - map.worldPosition.y;

            long chunkDifferenceX = (long)((map.centerCoordinates.indices.i + positionDifferenceX) / map.chunkSize);
            long chunkDifferenceY = (long)((map.centerCoordinates.indices.j + positionDifferenceY) / map.chunkSize);

            coordinates.chunk.x = map.centerCoordinates.chunk.x + chunkDifferenceX;
            coordinates.chunk.y = map.centerCoordinates.chunk.y + chunkDifferenceY;

            coordinates.indices.i = (int)(map.centerCoordinates.indices.i + positionDifferenceX - chunkDifferenceX * map.chunkSize);
            coordinates.indices.j = (int)(map.centerCoordinates.indices.j + positionDifferenceY - chunkDifferenceY * map.chunkSize);
        }
    }

}