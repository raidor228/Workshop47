using System.Collections.Generic;
using UnityEngine;

namespace Workshop47.Scripts.Game.State.Chunks
{
    public class ChunkData
    {
        public const int ChunkWidth = 16;
        public const int ChunkWidthSq = ChunkWidth * ChunkWidth;
        public const int ChunkHeight = 128;
        
        public Vector2Int Position { get; set; }
        public List<BlockData> Blocks { get; set; }
    }
}