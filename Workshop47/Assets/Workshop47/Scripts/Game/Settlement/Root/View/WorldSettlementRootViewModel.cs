using ObservableCollections;
using Workshop47.Scripts.Game.Settlement.Services;
using Workshop47.Scripts.Game.Settlement.View.Characters;
using Workshop47.Scripts.Game.Settlement.View.Player;
using R3;

namespace Workshop47.Scripts.Game.Settlement.Root.View
{
    public class WorldSettlementRootViewModel
    {
        public readonly IObservableCollection<CharacterViewModel> AllCharacters;
        public readonly ReadOnlyReactiveProperty<PlayerViewModel> Player;
        
        public WorldSettlementRootViewModel(CharactersService charactersService, PlayerService playerService)
        {
            AllCharacters = charactersService.AllCharacters;
            Player = playerService.Player;
        }
    }
}