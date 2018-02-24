using System;
using UnityEngine;
using Assets.Scripts.EditorAttributes;
using Assets.Scripts.Serialization;


namespace Assets.Scripts.Components
{
    public class TerrainTile : MonoBehaviour, IHasSerializableData<TerrainTileData>
    {

        [Serializable]
        public class Indices
        {
            [ReadOnly] public int i = 0;
            [ReadOnly] public int j = 0;

            public Indices(int i, int j)
            {
                this.i = i; this.j = j;
            }
        }

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

        public void RefreshName()
        {
            name = "Terrain Tile (" + indices.i + ", " + indices.j + ") : " + prefabIndex;
        }

        private void Update()
        {
            Chunk parentChunk = GetComponentInParent<Chunk>();
            if (parentChunk)
            {
                if (transform.position != parentChunk.CalculatedPositionForTileAt(indices))
                {
                    parentChunk.ResetTilePosition(indices);
                }
            }
        }

        [ContextMenu("Refresh")]
        private void RefreshFromMenu()
        {
            Chunk parentChunk = GetComponentInParent<Chunk>();
            if (parentChunk)
            {
                parentChunk.ReplaceTile(indices, prefabIndex);
            }
        }

    }

}

