using System;
using UnityEngine;
using Assets.Scripts.EditorAttributes;
using UnityEditor;


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

        private void Awake()
        {
            name = name + "(" + prefabIndex + ") [" + indices.i + ", " + indices.j + "]";
        }
    }

}

