using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Workshop47.Scripts.Game.Settings;
using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.State.Entities;
using Workshop47.Scripts.Game.State.Maps;
using Workshop47.Scripts.Game.State.Root;

namespace Workshop47.Scripts.Game.Gameplay.Commands.Handlers
{
    public class CmdCreateMapHandler : ICommandHandler<CmdCreateMap>
    {
        private readonly GameState _gameState;
        private readonly GameSettings _gameSettings;

        public CmdCreateMapHandler(GameState gameState, GameSettings gameSettings)
        {
            _gameState = gameState;
            _gameSettings = gameSettings;
        }

        public bool Handle(CmdCreateMap command)
        {
            var isMapAlreadyExisted = _gameState.Maps.Any(m => m.Id == command.MapId);

            if (isMapAlreadyExisted)
            {
                Debug.LogError($"Map with Id = {command.MapId} already exists");
                return false;
            }

            var newMapSettings = _gameSettings.MapsSettings.Maps.First(m => m.MapId == command.MapId);
            var newMapInitialStateSettings = newMapSettings.InitialStateSettings;

            var initialEntities = new List<EntityData>();
            foreach (var entitySettings in newMapInitialStateSettings.Entities)
            {
                var initialEntityData = EntitiesDataFactory.CreateEntity(entitySettings, _gameSettings.EntitiesSettings);
                initialEntityData.UniqueId = _gameState.CreateEntityId();
                initialEntities.Add(initialEntityData);
            }

            var newMapState = new MapData
            {
                Id = command.MapId,
                Entities = initialEntities
            };

            var newMapStateProxy = new Map(newMapState);

            _gameState.Maps.Add(newMapStateProxy);

            return true;
        }
    }
}