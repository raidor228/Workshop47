using System.Collections.Generic;
using ObservableCollections;
using UnityEngine;
using Workshop47.Scripts.Game.Settlement.View.Characters;
using R3;

namespace Workshop47.Scripts.Game.Settlement.Root.View
{
    public class WorldSettlementRootBinder : MonoBehaviour
    {
        private readonly Dictionary<int, CharacterBinder> _createdCharactersMap = new();
        
        private readonly CompositeDisposable _disposables = new();

        private WorldSettlementRootViewModel _viewModel;
        
        public void Bind(WorldSettlementRootViewModel viewModel)
        {
            _viewModel = viewModel;
            
            foreach (var buildingViewModel in viewModel.AllCharacters)
            {
                CreateBuilding(buildingViewModel);
            }
            
            _disposables.Add(viewModel.AllCharacters.ObserveAdd()
                .Subscribe(e => CreateBuilding(e.Value)));
            
            _disposables.Add(viewModel.AllCharacters.ObserveRemove()
                .Subscribe(e => DestroyBuilding(e.Value)));
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
        
        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}