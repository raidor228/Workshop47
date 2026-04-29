using System.Linq;
using UnityEngine;
using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.State.Entities;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Characters;
using Workshop47.Scripts.Game.State.Root;

namespace Workshop47.Scripts.Game.Gameplay.Commands.Handlers
{
    public class CmdPlaceEntityHandler : ICommandHandler<CmdPlaceEntity>
    {
        private readonly GameState _gameState;

        public CmdPlaceEntityHandler(GameState gameState)
        {
            _gameState = gameState;
        }
        
        public bool Handle(CmdPlaceEntity command)
        {
            var currentMap = _gameState.Maps.FirstOrDefault(m => m.Id == _gameState.CurrentMapId.CurrentValue);
            if (currentMap == null)
            {
                Debug.LogError($"Couldn't find MapState for id: {_gameState.CurrentMapId.CurrentValue}");
                return false;
            }
            
            var entityConfigId = command.EntityConfigId;
            var entityType = command.EntityType;
            var entityPosition = command.Position;
            var entityRotation = command.Rotation;
            var entityId = _gameState.CreateEntityId();
            EntityData createdEntityData = entityType switch
            {
                EntityType.Character => EntitiesDataFactory.CreateEntity<CharacterEntityData>(
                    entityType, entityConfigId,entityPosition, entityRotation),
                EntityType.Building => EntitiesDataFactory.CreateEntity<BuildingEntityData>(
                    entityType, entityConfigId,entityPosition, entityRotation),
                _ => throw new System.NotImplementedException(),
            };

            createdEntityData.UniqueId = entityId;
            var createEntity = EntitiesFactory.CreateEntity(createdEntityData);
            
            currentMap.Entities.Add(createEntity);

            return true;
        }
    }
}