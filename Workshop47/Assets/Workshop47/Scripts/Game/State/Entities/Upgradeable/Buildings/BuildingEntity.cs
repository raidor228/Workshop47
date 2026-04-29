using ObservableCollections;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Root;
using R3;

namespace Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings
{
    public class BuildingEntity : UpgradeableEntity
    {
        public ObservableList<IFeatureData> Features { get; } = new();
        
        public BuildingEntity(BuildingEntityData data) : base(data)
        {
            data.Features.ForEach(feature => Features.Add(feature));
            
            Features.ObserveAdd().Subscribe(e =>
            {
                var addedFeature = e.Value;
                data.Features.Add(addedFeature);
            });
            
            Features.ObserveRemove().Subscribe(e =>
            {
                var removedFeature = e.Value;
                data.Features.Remove(removedFeature);
            });
        }
        
        public T GetFeature<T>() where T : class, IFeatureData
        {
            foreach (var feature in Features)
            {
                if (feature is T typed)
                {
                    return typed;
                }
            }

            return null;
        }
    }
}