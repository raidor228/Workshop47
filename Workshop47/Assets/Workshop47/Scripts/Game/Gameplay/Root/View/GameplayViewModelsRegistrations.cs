using Melador.PlayerInput;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Gameplay.Services;
using Workshop47.Scripts.Game.Gameplay.Services.Features;
using Workshop47.Scripts.Game.Gameplay.View.UI;

namespace Workshop47.Scripts.Game.Gameplay.Root.View
{
    public static class GameplayViewModelsRegistrations
    {
        public static void Register(DIContainer container)
        {
            var playerInputProvider = container.Resolve<PlayerInputProvider>();
            var charactersService = container.Resolve<CharactersService>();
            var gameWorldService = container.Resolve<GameWorldService>();
            var playerService = container.Resolve<PlayerService>();
            var buildingService = container.Resolve<BuildingsService>();
            var onInteractablesOverlap = container.Resolve<PlayerService>().Player.CurrentValue.OnInteractablesOverlap;

            var gameplayUIManager = new GameplayUIManager(container);
            container.RegisterFactory(c => gameplayUIManager).AsSingle();
            container.RegisterFactory(c => new UIGameplayRootViewModel(playerInputProvider.InteractionsInput, onInteractablesOverlap)).AsSingle();
            container.RegisterFactory(c => new WorldGameplayRootViewModel(
                charactersService, gameWorldService, playerService, buildingService)).AsSingle();
        }
    }
}