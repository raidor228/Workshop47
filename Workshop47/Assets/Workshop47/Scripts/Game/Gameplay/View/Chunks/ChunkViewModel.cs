using System.Collections.Generic;
using UnityEngine;
using Workshop47.Scripts.Game.Gameplay.Services;
using Workshop47.Scripts.Game.Settings.Gameplay.Blocks;
using Workshop47.Scripts.Game.State.Chunks;

namespace Workshop47.Scripts.Game.Gameplay.View.Chunks
{
    public class ChunkViewModel
    {
        public readonly Vector2Int Position;
        public readonly List<BlockData> Blocks;
        
        public ChunkViewModel LeftChunk { get; private set; }
        public ChunkViewModel RightChunk { get; private set; }
        public ChunkViewModel ForwardChunk { get; private set; }
        public ChunkViewModel BackChunk { get; private set; }

        private readonly Dictionary<Vector3Int, BlockType> _blocksMap = new();
        
        private readonly Chunk _chunk;
        private readonly BlocksSettings _blocksSettings;
        private readonly GameWorldService _gameWorldService;
        
        public ChunkViewModel(Chunk chunk, BlocksSettings blocksSettings, 
            GameWorldService gameWorldService)
        {
            Position = chunk.Position;
            Blocks = chunk.Blocks;
            
            _chunk = chunk;
            _blocksSettings = blocksSettings;
            _gameWorldService = gameWorldService;

            foreach (var blockData in Blocks)
            {
                _blocksMap[blockData.Position] = blockData.BlockType;
            }
        }
        
        public BlockSettings GetBlockSettings(BlockType blockType)
        {
            return _gameWorldService.GetBlockSettings(blockType);
        }
        
        public BlockType GetBlock(Vector3Int blockPosition)
        {
            if (_blocksMap.TryGetValue(blockPosition, out var blockType))
            {
                return blockType;
            }

            return BlockType.Air;
        }
        
        public void SetNeighbours(ChunkViewModel left, ChunkViewModel right, 
            ChunkViewModel forward, ChunkViewModel back)
        {
            LeftChunk = left;
            RightChunk = right;
            ForwardChunk = forward;
            BackChunk = back;
        }
    }
}