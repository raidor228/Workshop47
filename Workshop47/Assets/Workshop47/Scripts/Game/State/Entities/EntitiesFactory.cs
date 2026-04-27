using System;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Characters;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Player;

namespace Workshop47.Scripts.Game.State.Entities
{
    public static class EntitiesFactory
    {
        public static Entity CreateEntity(EntityData entityData)
        {
            switch (entityData.Type)
            {
                case EntityType.Character:
                    return new CharacterEntity(entityData as CharacterEntityData);
                case EntityType.Player:
                    return new PlayerEntity(entityData as PlayerEntityData);
                default:
                    throw new Exception("Unsupported entity type: " + entityData.Type);
            }
        }
    }
}