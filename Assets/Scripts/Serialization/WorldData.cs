using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Serialization
{
    public class WorldData 
    {
        public class ChunkDictionaryPair
        {
            public LocationData locationData;
            public ChunkData chunkData;
        }

        ChunkDictionaryPair[] chunkDictionaryPair;
        
    }
}
