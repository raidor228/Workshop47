using UnityEngine;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings.Features
{
    public abstract class FeatureSettings : ScriptableObject
    {
        [field: SerializeField] public int Id { get; set; }
    }
}