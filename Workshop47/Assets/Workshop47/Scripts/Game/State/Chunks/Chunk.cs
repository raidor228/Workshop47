using System.Collections.Generic;
using UnityEngine;

namespace Workshop47.Scripts.Game.State.Chunks
{
    public class Chunk
    {
        public Vector2Int Position => _chunkData.Position;
        public List<BlockData> Blocks => _chunkData.Blocks;
        
        private readonly ChunkData _chunkData;
        
        public Chunk(ChunkData chunkData)
        {
            _chunkData = chunkData;
        }
    }
}