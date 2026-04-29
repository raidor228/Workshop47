using System.Collections.Generic;
using UnityEngine;
using Workshop47.Scripts.Game.State.Entities;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Entities
{
    public abstract class EntitySettings : ScriptableObject/*<T> : ScriptableObject where T : EntityLevelSettings*/
    {
        [field: SerializeField] public EntityType EntityType { get; private set; }
        [field: SerializeField] public string ConfigId { get; private set; }
        [field: SerializeField] public string TitleLid { get; private set; }
        [field: SerializeField] public string DescriptionLid { get; private set; }
        [field: SerializeField] public string PrefabPath { get; private set; }
    }
}