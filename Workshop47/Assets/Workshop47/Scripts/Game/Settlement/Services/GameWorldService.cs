using System.Collections.Generic;
using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.Settlement.View.Chunks;
using Workshop47.Scripts.Game.State.Chunks;
using Workshop47.Scripts.Game.Settings.Settlement.Blocks;
using Workshop47.Scripts.Game.Settings.Settlement.Worlds;
using UnityEngine;
using ObservableCollections;

namespace Workshop47.Scripts.Game.Settlement.Services
{
    public class GameWorldService
    {
        public IObservableCollection<ChunkViewModel> AllEditedChunks => _allEditedChunks;
        
        private readonly ObservableList<ChunkViewModel> _allEditedChunks = new();
        private readonly Dictionary<Vector2Int, ChunkViewModel> _chunksMap = new();
        private readonly Dictionary<BlockType, BlockSettings> _blocksSettingsMap = new();
        private readonly GameWorldSettings _gameWorldSettings;
        private readonly BlocksSettings _blocksSettings;
        private readonly ICommandProcessor _cmd;
        
        public GameWorldService(GameWorldSettings gameWorldSettings, BlocksSettings blocksSettings, 
            ICommandProcessor cmd)
        {
            _cmd = cmd;
            _gameWorldSettings = gameWorldSettings;
            _blocksSettings = blocksSettings;
            
            foreach (var blockSettings in blocksSettings.Blocks)
            {
                _blocksSettingsMap[blockSettings.BlockType] = blockSettings;
            }
            
            ChunkRenderer.InitTriangles();
            
            foreach (var chunkEntry in gameWorldSettings.Chunks)
            {
                var chunkData = new ChunkData()
                {
                    Position = chunkEntry.Position,
                    Blocks = chunkEntry.Blocks
                };
                var chunk = new Chunk(chunkData);
                CreateChunkViewModel(chunk);
            }

            SetChunkNeighbours();
        }
        
        public BlockSettings GetBlockSettings(BlockType blockType)
        {
            return _blocksSettingsMap[blockType];
        }
        
        private void SetChunkNeighbours()
        {
            foreach (var kvp in _chunksMap)
            {
                _chunksMap.TryGetValue(kvp.Key + Vector2Int.left, out var left);
                _chunksMap.TryGetValue(kvp.Key + Vector2Int.right, out var right);
                _chunksMap.TryGetValue(kvp.Key + Vector2Int.down, out var back);
                _chunksMap.TryGetValue(kvp.Key + Vector2Int.up, out var forward);
                
                kvp.Value.SetNeighbours(left, right, forward, back);
            }
        }
        
        private void CreateChunkViewModel(Chunk chunk)
        {
            var chunkViewModel = new ChunkViewModel(chunk, _blocksSettings, this);
            
            _allEditedChunks.Add(chunkViewModel);
            _chunksMap[chunk.Position] = chunkViewModel;
        }

        private void RemoveChunkViewModel(Chunk chunk)
        {
            if (_chunksMap.TryGetValue(chunk.Position, out var chunkViewModel))
            {
                _allEditedChunks.Remove(chunkViewModel);
                _chunksMap.Remove(chunk.Position);
            }
        }
    }
}