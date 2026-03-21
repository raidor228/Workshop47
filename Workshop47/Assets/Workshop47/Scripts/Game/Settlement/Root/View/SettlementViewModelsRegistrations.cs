using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Settlement.View.UI;

namespace Workshop47.Scripts.Game.Settlement.Root.View
{
    public static class SettlementViewModelsRegistrations
    {
        public static void Register(DIContainer container)
        {
            container.RegisterFactory(c => new SettlementUIManager(container)).AsSingle();
            container.RegisterFactory(c => new UISettlementRootViewModel()).AsSingle();
            container.RegisterFactory(c => new WorldSettlementRootViewModel()).AsSingle();
        }
    }
}