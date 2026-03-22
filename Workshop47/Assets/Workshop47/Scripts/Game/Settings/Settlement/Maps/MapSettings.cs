using UnityEngine;

namespace Workshop47.Scripts.Game.Settings.Settlement.Maps
{
    [CreateAssetMenu(fileName = "MapSettings", menuName = "Game Settings/Maps/New Map Settings")]
    public class MapSettings : ScriptableObject
    {
        public int MapId;
        public MapInitialStateSettings InitialStateSettings;
    }
}