using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.MainMenu.View.UI;

namespace Workshop47.Scripts.Game.MainMenu.Root.View
{
    public static class MainMenuViewModelsRegistrations
    {
        public static void Register(DIContainer container)
        {
            container.RegisterFactory(c => new MainMenuUIManager(container)).AsSingle();
            container.RegisterFactory(c => new UIMainMenuRootViewModel()).AsSingle();
            container.RegisterFactory(c => new WorldMainMenuRootViewModel()).AsSingle();
        }
    }
}