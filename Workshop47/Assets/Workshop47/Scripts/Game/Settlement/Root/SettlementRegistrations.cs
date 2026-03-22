using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Settings;
using Workshop47.Scripts.Game.State;
using Workshop47.Scripts.Game.State.Commands;
using R3;
using UnityEngine;
using Workshop47.Scripts.Game.Common;
using Workshop47.Scripts.Game.Settlement.Commands.Handlers;
using Workshop47.Scripts.Game.Settlement.Services;

namespace Workshop47.Scripts.Game.Settlement.Root
{
    public static class SettlementRegistrations
    {
        public static void Register(DIContainer container, SettlementEnterParams settlementEnterParams)
        {
            var gameStateProvider = container.Resolve<IGameStateProvider>();
            var gameState = gameStateProvider.GameState;
            var settingsProvider = container.Resolve<ISettingsProvider>();
            var gameSettings = settingsProvider.GameSettings;
            
            container.RegisterInstance(AppConstants.EXIT_SCENE_REQUEST_TAG, new Subject<Unit>());

            var cmd = new CommandProcessor(gameStateProvider);
            cmd.RegisterHandler(new CmdPlaceEntityHandler(gameState));
            container.RegisterInstance<ICommandProcessor>(cmd);

            var charactersService = new CharactersService(gameState.Entities, gameSettings.EntitiesSettings, cmd);
            container.RegisterFactory(_ => charactersService).AsSingle();

            //container.RegisterFactory(_ => new ResourcesService(gameState.Resources, cmd)).AsSingle();
        }
    }
}