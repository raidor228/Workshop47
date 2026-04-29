using System.Collections.Generic;
using UnityEngine;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Characters;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities.Player;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Entities
{
    [CreateAssetMenu(fileName = "EntitiesSettings", menuName = "Game Settings/Entities/New Entities Settings")]
    public class EntitiesSettings : ScriptableObject
    {
        [field: SerializeField] public List<CharacterSettings> Characters { get; private set; }
        [field: SerializeField] public List<BuildingSettings> Buildings { get; private set; }
        [field: SerializeField] public PlayerSettings Player { get; private set; }
    }
}