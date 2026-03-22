using System.Linq;
using Workshop47.Scripts.Game.State;
using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.State.GameResources;
using Workshop47.Scripts.Game.State.Root;

namespace Workshop47.Scripts.Game.Settlement.Commands.Handlers
{
    public class CmdResourcesAddHandler : ICommandHandler<CmdResourcesAdd>
    {
        private readonly GameState _gameState;

        public CmdResourcesAddHandler(GameState gameState)
        {
            _gameState = gameState;
        }
        
        public bool Handle(CmdResourcesAdd command)
        {
            var requiredResourceType = command.ResourceType;
            var requiredResource = _gameState.Resources.FirstOrDefault(r => r.ResourceType == requiredResourceType);
            if (requiredResource == null)
            {
                requiredResource = CreateNewResource(requiredResourceType);
            }

            requiredResource.Amount.Value += command.Amount;

            return true;
        }

        private Resource CreateNewResource(ResourceType resourceType)
        {
            var newResourceData = new ResourceData
            {
                ResourceType = resourceType,
                Amount = 0
            };

            var newResource = new Resource(newResourceData);
            _gameState.Resources.Add(newResource);

            
            return newResource;
        }
    }
}