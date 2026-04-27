using UnityEngine;
using Workshop47.Scripts.Game.Settings.Gameplay.Blocks;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities;
using Workshop47.Scripts.Game.Settings.Gameplay.Maps;

namespace Workshop47.Scripts.Game.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings/New Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public EntitiesSettings EntitiesSettings;
        public MapsSettings MapsSettings;
        public BlocksSettings BlocksSettings;
    }
}