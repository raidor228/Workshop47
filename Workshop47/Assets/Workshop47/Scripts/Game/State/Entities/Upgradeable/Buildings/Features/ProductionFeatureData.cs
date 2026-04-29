using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Root;

namespace Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Features
{
    public class ProductionFeatureData : IFeatureData
    {
        public int Id { get; set; }
        public float TimeRemaining { get; set; }
        public bool IsProducing { get; set; }
    }
}