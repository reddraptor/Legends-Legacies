﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Serialization
{
    [System.Serializable]
    public class ChunkData
    {
        public int tileSetIndex;
        public TerrainTileData[,] terrainTileData;
        public int size;
        
        public ChunkData(int tileSetIndex, int size )
        {
            this.tileSetIndex = tileSetIndex;
            this.size = size;
            terrainTileData = new TerrainTileData[size, size];
        }

        public override string ToString()
        {
            return
                "Serialization.ChunkData" + "\n" +
                "Tile Set Index: " + tileSetIndex + "\n" +
                "Size: " + size + "\n";
        }

    }
}
