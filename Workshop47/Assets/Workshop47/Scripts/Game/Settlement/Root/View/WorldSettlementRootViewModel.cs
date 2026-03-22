using ObservableCollections;
using Workshop47.Scripts.Game.Settlement.Services;
using Workshop47.Scripts.Game.Settlement.View.Characters;

namespace Workshop47.Scripts.Game.Settlement.Root.View
{
    public class WorldSettlementRootViewModel
    {
        public readonly IObservableCollection<CharacterViewModel> AllCharacters;
        
        public WorldSettlementRootViewModel(CharactersService charactersService)
        {
            AllCharacters = charactersService.AllCharacters;
        }
    }
}