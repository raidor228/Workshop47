using R3;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings.Features;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Features;

namespace Workshop47.Scripts.Game.Gameplay.View.Buildings.Features
{
    public class ProductionFeatureViewModel : FeatureViewModel
    {
        private readonly ProductionFeatureData _data;
        private readonly ProductionFeatureSettings _settings;

        public ProductionFeatureViewModel(ProductionFeatureData data,
            ProductionFeatureSettings settings)
        {
            _data = data;
            _settings = settings;
        }
    }
}