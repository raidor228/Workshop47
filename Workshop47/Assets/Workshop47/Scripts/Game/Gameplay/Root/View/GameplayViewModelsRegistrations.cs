using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Gameplay.Services;
using Workshop47.Scripts.Game.Gameplay.View.UI;

namespace Workshop47.Scripts.Game.Gameplay.Root.View
{
    public static class GameplayViewModelsRegistrations
    {
        public static void Register(DIContainer container)
        {
            var charactersService = container.Resolve<CharactersService>();
            var gameWorldService = container.Resolve<GameWorldService>();
            var playerService = container.Resolve<PlayerService>();
            
            container.RegisterFactory(c => new GameplayUIManager(container)).AsSingle();
            container.RegisterFactory(c => new UIGameplayRootViewModel()).AsSingle();
            container.RegisterFactory(c => new WorldGameplayRootViewModel(
                charactersService, gameWorldService, playerService)).AsSingle();
        }
    }
}