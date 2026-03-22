using UnityEngine;
using Workshop47.Scripts.Game.Settings.Settlement.Entities;

namespace Workshop47.Scripts.Game.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings/New Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public EntitiesSettings EntitiesSettings;
    }
}