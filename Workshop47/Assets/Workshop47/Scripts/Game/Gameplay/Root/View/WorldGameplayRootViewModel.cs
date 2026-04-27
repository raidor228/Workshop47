using ObservableCollections;
using R3;
using Workshop47.Scripts.Game.Gameplay.Services;
using Workshop47.Scripts.Game.Gameplay.View.Characters;
using Workshop47.Scripts.Game.Gameplay.View.Chunks;
using Workshop47.Scripts.Game.Gameplay.View.Player;

namespace Workshop47.Scripts.Game.Gameplay.Root.View
{
    public class WorldGameplayRootViewModel
    {
        public readonly IObservableCollection<CharacterViewModel> AllCharacters;
        public readonly IObservableCollection<ChunkViewModel> AllChunks;
        public readonly ReadOnlyReactiveProperty<PlayerViewModel> Player;
        
        public WorldGameplayRootViewModel(CharactersService charactersService, 
            GameWorldService gameWorldService, PlayerService playerService)
        {
            AllCharacters = charactersService.AllCharacters;
            AllChunks = gameWorldService.AllEditedChunks;
            Player = playerService.Player;
        }
    }
}