using Workshop47.Scripts.Game.Gameplay.View.Buildings;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings;

namespace Workshop47.Scripts.Game.Gameplay.Services.Features
{
    public interface IFeatureSystem
    {
        public void Tick(BuildingViewModel buildingViewModel, BuildingSettings buildingSettings, float deltaTime);
    }
}