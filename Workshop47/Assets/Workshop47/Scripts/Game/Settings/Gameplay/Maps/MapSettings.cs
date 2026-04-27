using UnityEngine;
using Workshop47.Scripts.Game.Settings.Gameplay.Worlds;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Maps
{
    [CreateAssetMenu(fileName = "MapSettings", menuName = "Game Settings/Maps/New Map Settings")]
    public class MapSettings : ScriptableObject
    {
        public int MapId;
        public GameWorldSettings GameWorldSettings;
        public MapInitialStateSettings InitialStateSettings;
    }
}