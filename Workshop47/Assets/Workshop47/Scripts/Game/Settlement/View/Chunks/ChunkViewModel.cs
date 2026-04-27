using UnityEngine;
using Workshop47.Scripts.Game.Settings.Settlement.Blocks;
using Workshop47.Scripts.Game.Settlement.Services;
using Workshop47.Scripts.Game.State.Chunks;

namespace Workshop47.Scripts.Game.Settlement.View.Chunks
{
    public class ChunkViewModel
    {
        public readonly Vector2Int Position;
        public readonly BlockType[] Blocks;
        
        public ChunkViewModel LeftChunk { get; private set; }
        public ChunkViewModel RightChunk { get; private set; }
        public ChunkViewModel ForwardChunk { get; private set; }
        public ChunkViewModel BackChunk { get; private set; }
        
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
        }
        
        public BlockSettings GetBlockSettings(BlockType blockType)
        {
            return _gameWorldService.GetBlockSettings(blockType);
        }
        
        public BlockType GetBlock(int x, int y, int z)
        {
            return _chunk.GetBlock(x, y, z);
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