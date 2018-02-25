using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "tile_set", menuName = "Virtual Raptor/TileSet")]
    public class TileSet : ScriptableObject
    {
        public GameObject[] terrainTilePrefabs;
        public int defaultIndex = 0;

        public string[] GetTilePrefabNames()
        {
            string[] prefabNames = new string[terrainTilePrefabs.Length];

            for (int i = 0; i < terrainTilePrefabs.Length; i++)
            {
                prefabNames[i] = terrainTilePrefabs[i].name;
            }

            return prefabNames;
        }
    }
}
