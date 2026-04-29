using System;
using System.Collections.Generic;
using UnityEngine;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Entities.Buildings.Features
{
    [CreateAssetMenu(fileName = "BuffFeatureSettings", menuName = "Game Settings/Entities/Buildings/Features/New Buff Feature Settings")]
    public class BuffFeatureSettings : FeatureSettings
    {
        [field: SerializeField] public List<Buff> Buffs { get; set; }
    }
    
    [Serializable]
    public class Buff
    {
        public string Target;
        public float Value;
    }
}