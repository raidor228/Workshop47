using System.Linq;
using UnityEngine;
using Workshop47.Scripts.Game.State;
using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.State.Root;

namespace Workshop47.Scripts.Game.Settlement.Commands.Handlers
{
    public class CmdResourcesSpendHandler : ICommandHandler<CmdResourcesSpend>
    {
        private readonly GameState _gameState;

        public CmdResourcesSpendHandler(GameState gameState)
        {
            _gameState = gameState;
        }

        public bool Handle(CmdResourcesSpend command)
        {
            var requiredResourceType = command.ResourceType;
            var requiredResource = _gameState.Resources.FirstOrDefault(r => r.ResourceType == requiredResourceType);
            if (requiredResource == null)
            {
                Debug.LogError("Trying to spend not existed resource");
                return false;
            }

            if (requiredResource.Amount.Value < command.Amount)
            {
                Debug.LogError(
                    $"Trying to spend more resources than existed ({requiredResourceType}). Exists: {requiredResource.Amount.Value}, trying to spend: {command.Amount}");
                return false;
            }

            requiredResource.Amount.Value -= command.Amount;

            return true;
        }
    }
}