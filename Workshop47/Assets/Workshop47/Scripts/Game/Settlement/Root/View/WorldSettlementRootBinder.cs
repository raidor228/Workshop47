using System.Collections.Generic;
using ObservableCollections;
using UnityEngine;
using Workshop47.Scripts.Game.Settlement.View.Characters;
using Workshop47.Scripts.Game.Player;
using Workshop47.Scripts.Game.Settlement.View.Chunks;
using Workshop47.Scripts.Game.Settlement.View.Player;
using R3;

namespace Workshop47.Scripts.Game.Settlement.Root.View
{
    public class WorldSettlementRootBinder : MonoBehaviour
    {
        private readonly Dictionary<int, CharacterBinder> _createdCharactersMap = new();
        private readonly Dictionary<Vector2Int, ChunkBinder> _createdChunksMap = new();
        private PlayerBinder _createdPlayer;
        
        private readonly CompositeDisposable _disposables = new();

        private WorldSettlementRootViewModel _viewModel;
        
        public void Bind(WorldSettlementRootViewModel viewModel)
        {
            _viewModel = viewModel;
            
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
            
            _disposables.Add(viewModel.Player.Subscribe(newPlayerViewModel =>
            {
                if (_createdPlayer != null)
                {
                    DestroyPlayer();
                }

                CreatePlayer(newPlayerViewModel);
            }));
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

        private void CreatePlayer(PlayerViewModel playerViewModel)
        {
            var playerLevel = playerViewModel.Level.CurrentValue;
            var prefabPlayerLevelPath = $"Prefabs/Settlement/Player/Player_{playerLevel}";
            var playerBinder = Resources.Load<PlayerBinder>(prefabPlayerLevelPath);
            var createdPlayer = Instantiate(playerBinder);
            
            createdPlayer.Bind(playerViewModel);

            _createdPlayer = createdPlayer;
        }

        private void DestroyPlayer()
        {
            Destroy(_createdPlayer);
            _createdPlayer = null;
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}