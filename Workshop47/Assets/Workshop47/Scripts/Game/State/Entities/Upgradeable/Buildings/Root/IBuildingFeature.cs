using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings.Features;

namespace Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Root
{
    public interface IBuildingFeature
    {
        FeatureSettings Settings { get; }
        IFeatureData Data { get; }
    }
}