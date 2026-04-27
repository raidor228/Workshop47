using System.Linq;
using UnityEngine;
using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.State.Root;

namespace Workshop47.Scripts.Game.Gameplay.Commands.Handlers
{
    public class CmdMoveEntityHandler : ICommandHandler<CmdMoveEntity>
    {
        private readonly GameState _gameState;

        public CmdMoveEntityHandler(GameState gameState)
        {
            _gameState = gameState;
        }
        
        public bool Handle(CmdMoveEntity command)
        {
            var currentMap = _gameState.Maps.FirstOrDefault(m => m.Id == _gameState.CurrentMapId.CurrentValue);
            if (currentMap == null)
            {
                Debug.LogError($"Couldn't find MapState for id: {_gameState.CurrentMapId.CurrentValue}");
                return false;
            }

            var entityId = command.EntityId;
            var entity = currentMap.Entities.First(e => e.UniqueId == entityId);
            entity.Position.Value = command.Position;

            return true;
        }
    }
}