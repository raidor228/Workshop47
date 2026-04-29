using System.Collections.Generic;
using ObservableCollections;
using Workshop47.Scripts.Game.Gameplay.Services;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings;
using UnityEngine;
using R3;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Root;

namespace Workshop47.Scripts.Game.Gameplay.View.Buildings
{
    public class BuildingViewModel
    {
        public readonly int EntityId;
        public readonly string ConfigId;

        public ReadOnlyReactiveProperty<Vector3> Position { get; }
        public ReadOnlyReactiveProperty<Vector3> Rotation { get; }
        public ReadOnlyReactiveProperty<int> Level { get; }
        public IObservableCollection<IFeatureData> Features { get; }
        
        private readonly BuildingEntity _buildingEntity;
        private readonly BuildingSettings _buildingSettings;
        private readonly BuildingsService _buildingsService;
        private readonly Dictionary<int, BuildingLevelSettings> _levelSettingsMap = new();
        
        public BuildingViewModel(BuildingEntity buildingEntity, 
            BuildingSettings buildingSettings, BuildingsService buildingsService)
        {
            EntityId = buildingEntity.UniqueId;
            ConfigId = buildingEntity.ConfigId;
            
            Level = buildingEntity.Level;
            Position = buildingEntity.Position;
            Rotation = buildingEntity.Rotation;
            Features = buildingEntity.Features;
            
            _buildingEntity = buildingEntity;
            _buildingSettings = buildingSettings;
            _buildingsService = buildingsService;
            
            foreach (var buildingLevelSettings in buildingSettings.Levels)
            {
                _levelSettingsMap[buildingLevelSettings.Level] = buildingLevelSettings;
            }
        }

        public BuildingLevelSettings GetLevelSettings(int level)
        {
            return _levelSettingsMap[level];
        }
    }
}