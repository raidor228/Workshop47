using System.Collections.Generic;
using UnityEngine;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Entities.Upgradeable
{
    public abstract class UpgradeableEntitySettings<T> : EntitySettings where T : UpgradeableEntityLevelSettings
    {
        [field: SerializeField] public List<T> Levels { get; private set; }
    }
}