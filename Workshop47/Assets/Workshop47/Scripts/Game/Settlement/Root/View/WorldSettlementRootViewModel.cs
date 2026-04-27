using ObservableCollections;
using R3;
using Workshop47.Scripts.Game.Settlement.Services;
using Workshop47.Scripts.Game.Settlement.View.Characters;
using Workshop47.Scripts.Game.Settlement.View.Chunks;
using Workshop47.Scripts.Game.Settlement.View.Player;

namespace Workshop47.Scripts.Game.Settlement.Root.View
{
    public class WorldSettlementRootViewModel
    {
        public readonly IObservableCollection<CharacterViewModel> AllCharacters;
        public readonly IObservableCollection<ChunkViewModel> AllChunks;
        public readonly ReadOnlyReactiveProperty<PlayerViewModel> Player;
        
        public WorldSettlementRootViewModel(CharactersService charactersService, 
            GameWorldService gameWorldService, PlayerService playerService)
        {
            AllCharacters = charactersService.AllCharacters;
            AllChunks = gameWorldService.AllEditedChunks;
            Player = playerService.Player;
        }
    }
}