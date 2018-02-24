using System;
using UnityEngine;
using Assets.Scripts.EditorAttributes;
using Assets.Scripts.Serialization;


namespace Assets.Scripts.Components
{
    public class TerrainTile : MonoBehaviour
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
        [ReadOnly]public int prefabIndex;

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
    }

}

