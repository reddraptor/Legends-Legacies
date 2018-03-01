using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Serialization
{
    [Serializable]
    public class TerrainTileData
    {
        public int prefabIndex;

        public TerrainTileData(int prefabIndex)
        {
            this.prefabIndex = prefabIndex;
        }
    }
}
