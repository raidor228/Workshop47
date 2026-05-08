using System;
using UnityEngine;

namespace Workshop47.Scripts.Game.State.Chunks
{
    [Serializable]
    public class BlockData
    {
        public Vector3Int Position { get; }
        public BlockType BlockType { get; }

        public BlockData(Vector3Int position, BlockType blockType)
        {
            Position = position;
            BlockType = blockType;
        }
    }
}