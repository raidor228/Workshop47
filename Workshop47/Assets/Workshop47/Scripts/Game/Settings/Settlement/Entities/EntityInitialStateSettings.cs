using System;
using UnityEngine;
using Workshop47.Scripts.Game.State.Entities;

namespace Workshop47.Scripts.Game.Settings.Settlement.Entities
{
    [Serializable]
    public class EntityInitialStateSettings
    {
        [field: SerializeField] public EntityType EntityType { get; private set; }
        [field: SerializeField] public string ConfigId { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public Vector3 InitialPosition { get; private set; }
        [field: SerializeField] public Vector3 InitialRotation { get; private set; }
    }
}