using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.State.Entities;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Characters;
using Workshop47.Scripts.Game.State.Root;

namespace Workshop47.Scripts.Game.Settlement.Commands.Handlers
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
            var entityConfigId = command.EntityConfigId;
            var entityType = command.EntityType;
            var entityPosition = command.Position;
            var entityRotation = command.Rotation;
            var entityId = _gameState.CreateEntityId();
            var createdEntityData = entityType switch
            {
                EntityType.Character => EntitiesDataFactory.CreateEntity<CharacterEntityData>(
                    entityType, entityConfigId,entityPosition, entityRotation),
                _ => throw new System.NotImplementedException(),
            };

            createdEntityData.UniqueId = entityId;
            var createEntity = EntitiesFactory.CreateEntity(createdEntityData);
            
            _gameState.Entities.Add(createEntity);

            return true;
        }
    }
}