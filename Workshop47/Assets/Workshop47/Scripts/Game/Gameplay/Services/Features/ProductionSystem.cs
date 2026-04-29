using System.Linq;
using UnityEngine;
using Workshop47.Scripts.Game.Gameplay.View.Buildings;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings.Features;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Features;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Root;

namespace Workshop47.Scripts.Game.Gameplay.Services.Features
{
    public class ProductionSystem : IFeatureSystem
    {
        private readonly ResourcesService _resourcesService;
        
        public ProductionSystem(ResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
        }
        
        public void Tick(BuildingViewModel buildingViewModel, BuildingSettings buildingSettings, 
            float deltaTime)
        {
            foreach (var featureData in buildingViewModel.Features)
            {
                int buildingLevel = buildingViewModel.Level.CurrentValue;
                var buildingLevelSettings = buildingSettings.Levels.First(s => s.Level == buildingLevel);
                var featureSettings = FindFeatureSettings(featureData, buildingLevelSettings);
                HandleFeature(
                    featureData as ProductionFeatureData, 
                    featureSettings as ProductionFeatureSettings,
                    deltaTime);
            }
        }

        private void HandleFeature(ProductionFeatureData data, ProductionFeatureSettings settings, 
            float deltaTime)
        {
            if (!data.IsProducing)
            {
                return;
            }
            
            data.TimeRemaining -= deltaTime;
            if (data.TimeRemaining <= 0)
            {
                data.TimeRemaining = settings.ProductionTime;
                if (_resourcesService.TrySpendResources(settings.Input.ResourceType, settings.Input.Amount))
                {
                    _resourcesService.AddResources(settings.Output.ResourceType, settings.Output.Amount);
                }
            }
        }
        
        private FeatureSettings FindFeatureSettings(IFeatureData data, BuildingLevelSettings buildingLevelSettings)
        {
            foreach (var feature in buildingLevelSettings.Features)
            {
                if (feature is ProductionFeatureSettings && data is ProductionFeatureData)
                {
                    if (feature.Id == data.Id)
                    {
                        return feature;
                    }
                }
            }

            return null;
        }
    }
}