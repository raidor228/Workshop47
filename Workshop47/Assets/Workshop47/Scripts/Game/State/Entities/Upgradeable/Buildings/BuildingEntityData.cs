using System.Collections.Generic;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings.Root;

namespace Workshop47.Scripts.Game.State.Entities.Upgradeable.Buildings
{
    public class BuildingEntityData : UpgradeableEntityData
    {
        public List<IFeatureData> Features { get; set; }
    }
}