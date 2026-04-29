using UnityEngine;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Upgradeable;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Entities.Player
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Game Settings/Player/New Player Settings")]
    public class PlayerSettings : UpgradeableEntitySettings<PlayerLevelSettings>
    {
    }
}