using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Settlement.Services;
using Workshop47.Scripts.Game.Settlement.View.UI;

namespace Workshop47.Scripts.Game.Settlement.Root.View
{
    public static class SettlementViewModelsRegistrations
    {
        public static void Register(DIContainer container)
        {
            var charactersService = container.Resolve<CharactersService>();
            var gameWorldService = container.Resolve<GameWorldService>();
            var playerService = container.Resolve<PlayerService>();
            
            container.RegisterFactory(c => new SettlementUIManager(container)).AsSingle();
            container.RegisterFactory(c => new UISettlementRootViewModel()).AsSingle();
            container.RegisterFactory(c => new WorldSettlementRootViewModel(
                charactersService, gameWorldService, playerService)).AsSingle();
        }
    }
}