using UnityEngine;

namespace Workshop47.Scripts.Game.State.Entities
{
    public class EntityData
    {
        public int UniqueId { get; set; }
        public string ConfigId { get; set; }
        public EntityType Type { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }
}