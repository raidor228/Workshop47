using R3;
using UnityEngine;

namespace Workshop47.Scripts.Game.State.Entities
{
    public abstract class Entity
    {
        public EntityData Origin { get; }
        public int UniqueId => Origin.UniqueId;
        public string ConfigId => Origin.ConfigId;
        public EntityType Type => Origin.Type;
        
        public readonly ReactiveProperty<Vector3> Position;
        public readonly ReactiveProperty<Vector3> Rotation;

        protected Entity(EntityData data)
        {
            Origin = data;

            Position = new ReactiveProperty<Vector3>(data.Position);
            Position.Subscribe(newPosition => { data.Position = newPosition; });
            
            Rotation = new ReactiveProperty<Vector3>(data.Rotation);
            Rotation.Subscribe(newRotation => { data.Rotation = newRotation; });
        }
    }
}