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

    }
}
