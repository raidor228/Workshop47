using System;
using UnityEngine;
using Workshop47.Scripts.Game.Settings.Settlement.Entities;
using Workshop47.Scripts.Game.State.Entities;

namespace Workshop47.Scripts.Game.Settlement.Commands.Handlers
{
    public static class EntitiesDataFactory
    {
        public static EntityData CreateEntity(EntityInitialStateSettings initialSettings)
        {
            switch (initialSettings.EntityType)
            {
                default:
                    throw new Exception($"Not implemented entity creation: {initialSettings.EntityType}");
            }
        }
        
        private static T CreateEntity<T>(EntityInitialStateSettings initialSettings) where T : EntityData, new()
        {
            return CreateEntity<T>(
                initialSettings.EntityType,
                initialSettings.ConfigId,
                initialSettings.InitialPosition,
                initialSettings.InitialRotation);
        }

        public static T CreateEntity<T>(EntityType type, string configId, Vector3 position, Vector3 rotation)
            where T : EntityData, new()
        {
            var entity = new T
            {
                Type = type,
                ConfigId = configId,
                Position = position,
                Rotation = rotation
            };

            switch (entity)
            {
                default:
                    throw new Exception($"Not implemented entity creation: {type}");
            }

            return entity;
        }
    }
}