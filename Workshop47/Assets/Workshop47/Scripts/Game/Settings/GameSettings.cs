using UnityEngine;
using Workshop47.Scripts.Game.Settings.Settlement.Blocks;
using Workshop47.Scripts.Game.Settings.Settlement.Entities;
using Workshop47.Scripts.Game.Settings.Settlement.Maps;

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