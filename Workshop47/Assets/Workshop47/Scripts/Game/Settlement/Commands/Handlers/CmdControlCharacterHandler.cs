using System.Linq;
using UnityEngine;
using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.State.Root;

namespace Workshop47.Scripts.Game.Settlement.Commands.Handlers
{
    public class CmdControlCharacterHandler : ICommandHandler<CmdControlCharacter>
    {
        private readonly GameState _gameState;

        public CmdControlCharacterHandler(GameState gameState)
        {
            _gameState = gameState;
        }
        
        public bool Handle(CmdControlCharacter command)
        {
            var currentMap = _gameState.Maps.FirstOrDefault(m => m.Id == _gameState.CurrentMapId.CurrentValue);
            if (currentMap == null)
            {
                Debug.LogError($"Couldn't find MapState for id: {_gameState.CurrentMapId.CurrentValue}");
                return false;
            }
            
            var characterId = command.CharacterId;
            bool exists = currentMap.Entities.Any(e => e.UniqueId == characterId);
            if (!exists)
            {
                Debug.LogError($"Couldn't find character with id: {characterId}");
                return false;
            }
            
            _gameState.ControllableEntityId.Value = characterId;

            return true;
        }
    }
}