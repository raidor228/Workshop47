using System;
using UnityEngine;
using Workshop47.Scripts.Game.State.GameResources;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings.Features
{
    [CreateAssetMenu(fileName = "ProductionFeatureSettings", menuName = "Game Settings/Entities/Buildings/Features/New Production Feature Settings")]
    public class ProductionFeatureSettings : FeatureSettings
    {
        [field: SerializeField] public ResourceEntry Input { get; set; }
        [field: SerializeField] public ResourceEntry Output { get; set; }
        [field: SerializeField] public float ProductionTime { get; set; }
    }

    [Serializable]
    public class ResourceEntry
    {
        public ResourceType ResourceType;
        public int Amount;
    }
}