using System;
using System.Collections.Generic;
using ObservableCollections;
using UnityEngine;
using Workshop47.Scripts.Game.Settlement.View.Characters;
using R3;
using Workshop47.Scripts.Game.Settlement.View.Chunks;

namespace Workshop47.Scripts.Game.Settlement.Root.View
{
    public class WorldSettlementRootBinder : MonoBehaviour
    {
        private readonly Dictionary<int, CharacterBinder> _createdCharactersMap = new();
        private readonly Dictionary<Vector2Int, ChunkBinder> _createdChunksMap = new();
        private Player.Player _player;
        
        private readonly CompositeDisposable _disposables = new();

        private WorldSettlementRootViewModel _viewModel;
        
        public void Bind(WorldSettlementRootViewModel viewModel)
        {
            _viewModel = viewModel;
            _player = new Player.Player(viewModel.PlayerInputActions);
            
            foreach (var buildingViewModel in viewModel.AllCharacters)
            {
                CreateBuilding(buildingViewModel);
            }
            
            foreach (var chunkViewModel in viewModel.AllChunks)
            {
                CreateChunk(chunkViewModel);
            }
            
            _disposables.Add(viewModel.AllCharacters.ObserveAdd()
                .Subscribe(e => CreateBuilding(e.Value)));
            
            _disposables.Add(viewModel.AllCharacters.ObserveRemove()
                .Subscribe(e => DestroyBuilding(e.Value)));
            
            _disposables.Add(viewModel.AllChunks.ObserveAdd()
                .Subscribe(e => CreateChunk(e.Value)));
            
            _disposables.Add(viewModel.AllChunks.ObserveRemove()
                .Subscribe(e => DestroyChunk(e.Value)));
            
            _disposables.Add(viewModel.ControllableCharacter.Skip(1).Subscribe(ControlCharacter));
        }

        private void CreateChunk(ChunkViewModel chunkViewModel)
        {
            var prefabChunkPath = $"Prefabs/Settlement/Chunks/ChunkRenderer";
            var chunkBinder = Resources.Load<ChunkBinder>(prefabChunkPath);
            var createdChunk = Instantiate(chunkBinder);
            
            createdChunk.Bind(chunkViewModel);

            _createdChunksMap[chunkViewModel.Position] = createdChunk;
        }

        private void DestroyChunk(ChunkViewModel chunkViewModel)
        {
            if (_createdChunksMap.TryGetValue(chunkViewModel.Position, out var chunkBinder))
            {
                Destroy(chunkBinder.gameObject);
                _createdChunksMap.Remove(chunkViewModel.Position);
            }
        }
        
        private void CreateBuilding(CharacterViewModel characterViewModel)
        {
            var characterLevel = characterViewModel.Level.CurrentValue;
            var characterType = characterViewModel.ConfigId;
            var prefabCharacterLevelPath = $"Prefabs/Settlement/Characters/Character_{characterType}_{characterLevel}";
            var characterBinder = Resources.Load<CharacterBinder>(prefabCharacterLevelPath);
            var createdCharacter = Instantiate(characterBinder);
            
            createdCharacter.Bind(characterViewModel);

            _createdCharactersMap[characterViewModel.EntityId] = createdCharacter;
        }

        private void DestroyBuilding(CharacterViewModel characterViewModel)
        {
            if (_createdCharactersMap.TryGetValue(characterViewModel.EntityId, out var characterBinder))
            {
                Destroy(characterBinder.gameObject);
                _createdCharactersMap.Remove(characterViewModel.EntityId);
            }
        }

        private void ControlCharacter(CharacterViewModel characterViewModel)
        {
            var binder = _createdCharactersMap[characterViewModel.EntityId];
            _player.SetCharacter(binder);
        }

        private void Update()
        {
            _player?.Update();
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
            _player.Dispose();
        }
    }
}