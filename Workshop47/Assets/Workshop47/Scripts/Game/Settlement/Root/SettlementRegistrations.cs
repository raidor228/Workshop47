using System;
using System.Linq;
using Melador.PlayerInput;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Settings;
using Workshop47.Scripts.Game.State;
using Workshop47.Scripts.Game.State.Commands;
using R3;
using UnityEngine;
using Workshop47.Scripts.Game.Common;
using Workshop47.Scripts.Game.Settlement.Commands;
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
            var playerInputProvider = container.Resolve<PlayerInputProvider>();
            
            container.RegisterInstance(AppConstants.EXIT_SCENE_REQUEST_TAG, new Subject<Unit>());

            var cmd = new CommandProcessor(gameStateProvider);
            cmd.RegisterHandler(new CmdPlaceEntityHandler(gameState));
            cmd.RegisterHandler(new CmdCreateMapHandler(gameState, gameSettings));
            cmd.RegisterHandler(new CmdMoveEntityHandler(gameState));
            container.RegisterInstance<ICommandProcessor>(cmd);

            var loadingMapId = settlementEnterParams.MapId;
            var loadingMap = gameState.Maps.FirstOrDefault(m => m.Id == loadingMapId);
            if (loadingMap == null)
            {
                var command = new CmdCreateMap(loadingMapId);
                var success = cmd.Process(command);
                if (!success)
                {
                    throw new Exception($"Couldn't create map state with id: ${loadingMapId}");
                }

                loadingMap = gameState.Maps.First(m => m.Id == loadingMapId);
            }
            
            var charactersService = new CharactersService(loadingMap.Entities, gameSettings.EntitiesSettings, cmd);
            container.RegisterFactory(_ => charactersService).AsSingle();
            
            var playerService = new PlayerService(gameState.Player, 
                gameSettings.EntitiesSettings.Player, playerInputProvider, cmd);
            container.RegisterFactory(_ => playerService).AsSingle();
            playerService.EnablePlayerInput(true);
            
            var loadingMapSettings = gameSettings.MapsSettings.Maps.First(m => m.MapId == loadingMapId);
            var gameWorldService = new GameWorldService(loadingMapSettings.GameWorldSettings, gameSettings.BlocksSettings, cmd);
            container.RegisterFactory(_ => gameWorldService).AsSingle();
            
            //container.RegisterFactory(_ => new ResourcesService(gameState.Resources, cmd)).AsSingle();
        }
    }
}