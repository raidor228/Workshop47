using UnityEngine;

namespace Workshop47.Scripts.Game.State.Chunks
{
    public class Chunk
    {
        public Vector2Int Position => _chunkData.Position;
        public BlockType[] Blocks => _chunkData.Blocks;
        
        private readonly ChunkData _chunkData;
        
        public Chunk(ChunkData chunkData)
        {
            _chunkData = chunkData;
        }

        public BlockType GetBlock(int x, int y, int z)
        {
            return _chunkData.GetBlock(x, y, z);
        }
    }
}