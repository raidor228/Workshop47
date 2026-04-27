using UnityEngine;

namespace Workshop47.Scripts.Game.State.Chunks
{
    public class ChunkData
    {
        public const int ChunkWidth = 16;
        public const int ChunkWidthSq = ChunkWidth * ChunkWidth;
        public const int ChunkHeight = 128;
        
        public Vector2Int Position { get; set; }
        public BlockType[] Blocks { get; set; }

        public BlockType GetBlock(int x, int y, int z)
        {
            int index = x + y * ChunkWidthSq + z * ChunkWidth;
            return Blocks[index];
        }
    }
}