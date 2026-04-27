using ObservableCollections;
using R3;
using Workshop47.Input;
using Workshop47.Scripts.Game.Settlement.Services;
using Workshop47.Scripts.Game.Settlement.View.Characters;
using Workshop47.Scripts.Game.Settlement.View.Chunks;

namespace Workshop47.Scripts.Game.Settlement.Root.View
{
    public class WorldSettlementRootViewModel
    {
        public readonly IObservableCollection<CharacterViewModel> AllCharacters;
        public readonly IObservableCollection<ChunkViewModel> AllChunks;
        public readonly ReadOnlyReactiveProperty<CharacterViewModel> ControllableCharacter;
        
        public readonly PlayerInputActions PlayerInputActions;
        
        public WorldSettlementRootViewModel(PlayerInputActions playerInputActions, 
            CharactersService charactersService, GameWorldService gameWorldService,
            ReadOnlyReactiveProperty<CharacterViewModel> controllableCharacter)
        {
            PlayerInputActions = playerInputActions;
            
            AllCharacters = charactersService.AllCharacters;
            AllChunks = gameWorldService.AllEditedChunks;
            ControllableCharacter = controllableCharacter;
        }
    }
}