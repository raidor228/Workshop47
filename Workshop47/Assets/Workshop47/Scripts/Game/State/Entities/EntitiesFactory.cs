using System;

namespace Workshop47.Scripts.Game.State.Entities
{
    public static class EntitiesFactory
    {
        public static Entity CreateEntity(EntityData entityData)
        {
            switch (entityData.Type)
            {
                default:
                    throw new Exception("Unsupported entity type: " + entityData.Type);
            }
        }
    }
}