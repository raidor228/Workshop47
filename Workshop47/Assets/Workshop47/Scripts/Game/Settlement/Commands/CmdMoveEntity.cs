using UnityEngine;
using Workshop47.Scripts.Game.State.Commands;

namespace Workshop47.Scripts.Game.Settlement.Commands
{
    public class CmdMoveEntity : ICommand
    {
        public readonly int EntityId;
        public readonly Vector3 Position;
        
        public CmdMoveEntity(int entityId, Vector3 position)
        {
            EntityId = entityId;
            Position = position;
        }
    }
}