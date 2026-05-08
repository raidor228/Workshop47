using System.Collections.Generic;
using UnityEngine;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings.Features;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Upgradeable;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings
{
    [CreateAssetMenu(fileName = "BuildingLevelSettings", menuName = "Game Settings/Entities/Buildings/New Building Level Settings")]
    public class BuildingLevelSettings : UpgradeableEntityLevelSettings
    {
        [field: SerializeField] public List<FeatureSettings> Features { get; set; }
        [field: SerializeField] public BuildingViewSettings ViewSettings { get; set; }
    }
}