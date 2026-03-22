using System.Collections.Generic;
using UnityEngine;
using Workshop47.Scripts.Game.Settings.Settlement.Entities.Characters;

namespace Workshop47.Scripts.Game.Settings.Settlement.Entities
{
    [CreateAssetMenu(fileName = "EntitiesSettings", menuName = "Game Settings/Entities/New Entities Settings")]
    public class EntitiesSettings : ScriptableObject
    {
        [field: SerializeField] public List<CharacterSettings> Characters { get; private set; }
    }
}