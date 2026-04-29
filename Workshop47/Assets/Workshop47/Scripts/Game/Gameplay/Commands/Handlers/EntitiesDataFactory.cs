using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings.Features;
using Workshop47.Scripts.Game.State.Entities;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Features;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Root;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Characters;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Player;

namespace Workshop47.Scripts.Game.Gameplay.Commands.Handlers
{
    public static class EntitiesDataFactory
    {
        public static EntityData CreateEntity(EntityInitialStateSettings initialSettings, 
            EntitiesSettings entitiesSettings)
        {
            switch (initialSettings.EntityType)
            {
                case EntityType.Character:
                    return CreateEntity<CharacterEntityData>(initialSettings, entitiesSettings);
                case EntityType.Player:
                    return CreateEntity<PlayerEntityData>(initialSettings, entitiesSettings);
                case EntityType.Building:
                    return CreateEntity<BuildingEntityData>(initialSettings, entitiesSettings);
                default:
                    throw new Exception($"Not implemented entity creation: {initialSettings.EntityType}");
            }
        }
        
        private static T CreateEntity<T>(EntityInitialStateSettings initialSettings, 
            EntitiesSettings entitiesSettings) where T : EntityData, new()
        {
            return CreateEntity<T>(
                initialSettings.EntityType,
                initialSettings.ConfigId,
                initialSettings.InitialPosition,
                initialSettings.InitialRotation, 
                entitiesSettings);
        }

        public static T CreateEntity<T>(EntityType type, string configId, Vector3 position, Vector3 rotation, 
            EntitiesSettings entitiesSettings) where T : EntityData, new()
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
                case PlayerEntityData playerEntityData:
                    UpdatePlayerEntity(playerEntityData, entitiesSettings);
                    break;
                case CharacterEntityData characterEntityData:
                    UpdateCharacterEntity(characterEntityData, entitiesSettings);
                    break;
                case BuildingEntityData buildingEntityData:
                    UpdateBuildingEntity(buildingEntityData, entitiesSettings);
                    break;
                default:
                    throw new Exception($"Not implemented entity creation: {type}");
            }

            return entity;
        }
        
        private static void UpdateCharacterEntity(CharacterEntityData characterEntity, 
            EntitiesSettings entitiesSettings)
        {
            var settings = entitiesSettings.Characters.First(b => b.ConfigId == characterEntity.ConfigId);

        }
        
        private static void UpdatePlayerEntity(PlayerEntityData playerEntity, 
            EntitiesSettings entitiesSettings)
        {
            
        }
        
        private static void UpdateBuildingEntity(BuildingEntityData buildingEntity, 
            EntitiesSettings entitiesSettings)
        {
            buildingEntity.Features = new List<IFeatureData>();
            
            var buildingSettings = entitiesSettings.Buildings.First(b => b.ConfigId == buildingEntity.ConfigId);
            if (buildingSettings.Levels.Count > 0)
            {
                buildingEntity.Level = 1;
                var buildingLevelSettings = buildingSettings.Levels[0];
                foreach (var settingsFeature in buildingLevelSettings.Features)
                {
                    var featureData = FeaturesFactory.CreateFeature(settingsFeature);
                    featureData.Id = settingsFeature.Id;
                    buildingEntity.Features.Add(featureData);
                }
            }
        }
    }
}