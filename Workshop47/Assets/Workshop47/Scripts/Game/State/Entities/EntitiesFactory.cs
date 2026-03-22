using System;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Characters;

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
                default:
                    throw new Exception("Unsupported entity type: " + entityData.Type);
            }
        }
    }
}