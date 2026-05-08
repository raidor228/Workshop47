using ObservableCollections;
using R3;
using Workshop47.Scripts.Game.Gameplay.Services;
using Workshop47.Scripts.Game.Gameplay.View.Buildings;
using Workshop47.Scripts.Game.Gameplay.View.Characters;
using Workshop47.Scripts.Game.Gameplay.View.Chunks;
using Workshop47.Scripts.Game.Gameplay.View.Player;

namespace Workshop47.Scripts.Game.Gameplay.Root.View
{
    public class WorldGameplayRootViewModel
    {
        public readonly IObservableCollection<CharacterViewModel> AllCharacters;
        public readonly IObservableCollection<BuildingViewModel> AllBuildings;
        public readonly IObservableCollection<ChunkViewModel> AllChunks;
        public readonly ReadOnlyReactiveProperty<PlayerViewModel> Player;

        private readonly BuildingsService _buildingsService;
        
        public WorldGameplayRootViewModel(CharactersService charactersService, 
            GameWorldService gameWorldService, PlayerService playerService, 
            BuildingsService buildingsService)
        {
            AllCharacters = charactersService.AllCharacters;
            AllBuildings = buildingsService.AllBuildings;
            AllChunks = gameWorldService.AllChunks;
            Player = playerService.Player;

            _buildingsService = buildingsService;
        }

        public void OnUpdate(float deltaTime)
        {
            _buildingsService.OnTick(deltaTime);
        }
    }
}