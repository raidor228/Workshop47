using System;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings.Features;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Features;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Root;

namespace Workshop47.Scripts.Game.Gameplay.Commands.Handlers
{
    public static class FeaturesFactory
    {
        public static IFeatureData CreateFeature(FeatureSettings featureSettings)
        {
            IFeatureData featureData = featureSettings switch
            {
                BuffFeatureSettings buffFeatureSettings => new BuffFeatureData(),
                ProductionFeatureSettings productionFeatureSettings => new ProductionFeatureData()
                {
                    IsProducing = true,
                    TimeRemaining = productionFeatureSettings.ProductionTime
                },
                _ => throw new Exception($"Not implemented feature creation: {featureSettings.GetType()}")
            };
            
            return featureData;
        }
    }
}