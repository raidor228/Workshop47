using System;
using System.Collections.Generic;
using ObservableCollections;
using R3;
using UnityEngine;
using Workshop47.Scripts.Game.Gameplay.Commands;
using Workshop47.Scripts.Game.Gameplay.Services.Features;
using Workshop47.Scripts.Game.Gameplay.View.Buildings;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings;
using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.State.Entities;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings;

namespace Workshop47.Scripts.Game.Gameplay.Services
{
    public class BuildingsService
    {
        public IObservableCollection<BuildingViewModel> AllBuildings => _allBuildings;

        private readonly ObservableList<BuildingViewModel> _allBuildings = new();
        private readonly Dictionary<int, BuildingViewModel> _buildingsMap = new();
        private readonly Dictionary<string, BuildingSettings> _buildingSettingsMap = new();
        private readonly List<IFeatureSystem> _featureSystems = new();
        private readonly ICommandProcessor _cmd;
        
        public BuildingsService(IObservableCollection<Entity> entities, 
            EntitiesSettings entitiesSettings, ResourcesService resourcesService, ICommandProcessor cmd)
        {
            _cmd = cmd;
            
            foreach (var buildingSettings in entitiesSettings.Buildings)
            {
                _buildingSettingsMap[buildingSettings.ConfigId] = buildingSettings;
            }

            foreach (var entity in entities)
            {
                if (entity is BuildingEntity buildingEntity)
                {
                    CreateBuildingViewModel(buildingEntity);
                }
            }

            entities.ObserveAdd().Subscribe(e =>
            {
                var entity = e.Value;
                if (entity is BuildingEntity buildingEntity)
                {
                    CreateBuildingViewModel(buildingEntity);
                }
            });

            entities.ObserveRemove().Subscribe(e =>
            {
                var entity = e.Value;
                if (entity is BuildingEntity buildingEntity)
                {
                    RemoveBuildingViewModel(buildingEntity);
                }
            });
            
            _featureSystems.Add(new ProductionSystem(resourcesService));
            _featureSystems.Add(new BuffSystem());
        }

        public bool PlaceBuilding(string buildingConfigId, Vector3 position, Vector3 rotation)
        {
            var command = new CmdPlaceEntity(EntityType.Building, buildingConfigId, position, rotation);
            var result = _cmd.Process(command);

            return result;
        }
        
        public bool DeleteBuilding(int buildingEntityId)
        {
            throw new NotImplementedException();
        }
        
        public void OnTick(float deltaTime)
        {
            foreach (var featureSystem in _featureSystems)
            {
                foreach (var buildingViewModel in _allBuildings)
                {
                    if (_buildingSettingsMap.TryGetValue(buildingViewModel.ConfigId, out var buildingSettings))
                    {
                        featureSystem.Tick(buildingViewModel, buildingSettings, deltaTime);
                    }
                    else
                    {
                        Debug.Log($"Could not find building settings with config id: {buildingViewModel.ConfigId}");
                    }
                }
            }
        }

        private void CreateBuildingViewModel(BuildingEntity buildingEntity)
        {
            var buildingSettings = _buildingSettingsMap[buildingEntity.ConfigId];
            var buildingViewModel = new BuildingViewModel(buildingEntity, buildingSettings, this);
            
            _allBuildings.Add(buildingViewModel);
            _buildingsMap[buildingEntity.UniqueId] = buildingViewModel;
        }

        private void RemoveBuildingViewModel(BuildingEntity buildingEntity)
        {
            if (_buildingsMap.TryGetValue(buildingEntity.UniqueId, out var buildingViewModel))
            {
                _allBuildings.Remove(buildingViewModel);
                _buildingsMap.Remove(buildingEntity.UniqueId);
            }
        }
    }
}