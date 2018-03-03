using System;
using UnityEngine;
using Assets.Scripts.EditorAttributes;
using Assets.Scripts.Serialization;
using Assets.Scripts.Helpers;

namespace Assets.Scripts.Components
{
    public class TerrainTileComponent : MonoBehaviour, IHasSerializableData<TerrainTileData>
    {

        public enum TerrainType { Land, Water, Wall }

        public Indices indices;
        public TerrainType terrainType;
        public float speedModifier;
        public int prefabIndex;

        public TerrainTileData GetSerializableData()
        {
            return new TerrainTileData(prefabIndex);
        }

        public void SetFromSerializableData(TerrainTileData data)
        {
            prefabIndex = data.prefabIndex;
        }

        private void Start()
        {
            GetComponent<BoxCollider2D>().enabled = false;
            name = name + "(" + prefabIndex + ") [" + indices.i + ", " + indices.j + "]";
        }
        
    }

}

