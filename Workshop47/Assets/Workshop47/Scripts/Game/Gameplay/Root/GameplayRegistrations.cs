using System;
using System.Linq;
using Melador.PlayerInput;
using R3;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Common;
using Workshop47.Scripts.Game.Gameplay.Services;
using Workshop47.Scripts.Game.Settings;
using Workshop47.Scripts.Game.Gameplay.Commands;
using Workshop47.Scripts.Game.Gameplay.Commands.Handlers;
using Workshop47.Scripts.Game.State;
using Workshop47.Scripts.Game.State.Commands;

namespace Workshop47.Scripts.Game.Gameplay.Root
{
    public static class GameplayRegistrations
    {
        public static void Register(DIContainer container, GameplayEnterParams gameplayEnterParams)
        {
            var gameStateProvider = container.Resolve<IGameStateProvider>();
            var gameState = gameStateProvider.GameState;
            var settingsProvider = container.Resolve<ISettingsProvider>();
            var gameSettings = settingsProvider.GameSettings;
            var playerInputProvider = container.Resolve<PlayerInputProvider>();
            
            container.RegisterInstance(AppConstants.EXIT_SCENE_REQUEST_TAG, new Subject<Unit>());

            var cmd = new CommandProcessor(gameStateProvider);
            cmd.RegisterHandler(new CmdPlaceEntityHandler(gameState, gameSettings));
            cmd.RegisterHandler(new CmdCreateMapHandler(gameState, gameSettings));
            cmd.RegisterHandler(new CmdMoveEntityHandler(gameState));
            cmd.RegisterHandler(new CmdResourcesAddHandler(gameState));
            cmd.RegisterHandler(new CmdResourcesSpendHandler(gameState));
            container.RegisterInstance<ICommandProcessor>(cmd);

            var loadingMapId = gameplayEnterParams.MapId;
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

            var resourcesService = new ResourcesService(gameState.Resources, cmd);
            container.RegisterFactory(_ => resourcesService).AsSingle();
            
            var buildingService = new BuildingsService(loadingMap.Entities, gameSettings.EntitiesSettings, 
                resourcesService, cmd);
            container.RegisterFactory(_ => buildingService).AsSingle();
        }
    }
}