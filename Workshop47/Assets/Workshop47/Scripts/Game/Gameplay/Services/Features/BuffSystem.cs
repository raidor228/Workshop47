using System.Linq;
using Workshop47.Scripts.Game.Gameplay.View.Buildings;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings.Features;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Features;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Root;

namespace Workshop47.Scripts.Game.Gameplay.Services.Features
{
    public class BuffSystem : IFeatureSystem
    {
        public BuffSystem()
        {
            
        }
        
        public void Tick(BuildingViewModel buildingViewModel, BuildingSettings buildingSettings, 
            float deltaTime)
        {
            foreach (var featureData in buildingViewModel.Features)
            {
                int buildingLevel = buildingViewModel.Level.CurrentValue;
                var buildingLevelSettings = buildingSettings.Levels.First(s => s.Level == buildingLevel);
                var featureSettings = FindFeatureSettings(featureData, buildingLevelSettings);
                
                var buffFeatureData = featureData as BuffFeatureData;
                var buffFeatureSettings = featureSettings as BuffFeatureSettings;
                if (buffFeatureData == null)
                {
                    return;
                }
                
                HandleFeature(buffFeatureData, buffFeatureSettings, deltaTime);
            }
        }

        private void HandleFeature(BuffFeatureData data, BuffFeatureSettings settings, 
            float deltaTime)
        {
            
        }
        
        private FeatureSettings FindFeatureSettings(IFeatureData data, BuildingLevelSettings buildingLevelSettings)
        {
            foreach (var feature in buildingLevelSettings.Features)
            {
                if (feature is BuffFeatureSettings && data is BuffFeatureData)
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