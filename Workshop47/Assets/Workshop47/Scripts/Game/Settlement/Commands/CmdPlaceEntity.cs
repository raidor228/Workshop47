using UnityEngine;
using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.State.Entities;

namespace Workshop47.Scripts.Game.Settlement.Commands
{
    public class CmdPlaceEntity : ICommand
    {
        public readonly EntityType EntityType;
        public readonly string EntityConfigId;
        public readonly Vector3 Position;
        public readonly Vector3 Rotation;
        
        public CmdPlaceEntity(EntityType entityType, string entityConfigId, Vector3 position, Vector3 rotation)
        {
            EntityType = entityType;
            EntityConfigId = entityConfigId;
            Position = position;
            Rotation = rotation;
        }
    }
}