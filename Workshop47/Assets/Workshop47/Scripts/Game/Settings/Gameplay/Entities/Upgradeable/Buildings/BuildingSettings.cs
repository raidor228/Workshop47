using System.Collections.Generic;
using UnityEngine;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings.Features;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Upgradeable;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings
{
    [CreateAssetMenu(fileName = "BuildingSettings", menuName = "Game Settings/Entities/Buildings/New Building Settings")]
    public class BuildingSettings : UpgradeableEntitySettings<BuildingLevelSettings>
    {

    }
}