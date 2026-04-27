using UnityEngine;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Entities.Characters
{
    [CreateAssetMenu(fileName = "CharacterSettings", menuName = "Game Settings/Characters/New Character Settings")]
    public class CharacterSettings : EntitySettings<CharacterLevelSettings>
    {
        [field: SerializeField] public float Speed { get; private set; }
    }
}