using R3;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Common;
using Workshop47.Scripts.Game.Settings;
using Workshop47.Scripts.Game.State;
using Workshop47.Scripts.Game.State.Commands;

namespace Workshop47.Scripts.Game.MainMenu.Root
{
    public static class MainMenuRegistrations
    {
        public static void Register(DIContainer container, MainMenuEnterParams mainMenuEnterParams)
        {
            var gameStateProvider = container.Resolve<IGameStateProvider>();
            var gameState = gameStateProvider.GameState;
            var settingsProvider = container.Resolve<ISettingsProvider>();
            var gameSettings = settingsProvider.GameSettings;
            
            container.RegisterInstance(AppConstants.EXIT_SCENE_REQUEST_TAG, new Subject<Unit>());
            
            var cmd = new CommandProcessor(gameStateProvider);
            //cmd.RegisterHandler(new CmdPlaceEntityHandler(gameState));
            container.RegisterInstance<ICommandProcessor>(cmd);
        }
    }
}