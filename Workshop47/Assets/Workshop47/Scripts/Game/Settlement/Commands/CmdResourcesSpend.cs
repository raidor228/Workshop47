using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.State.GameResources;

namespace Workshop47.Scripts.Game.Settlement.Commands
{
    public class CmdResourcesSpend : ICommand
    {
        public readonly ResourceType ResourceType;
        public readonly int Amount;
        
        public CmdResourcesSpend(ResourceType resourceType, int amount)
        {
            ResourceType = resourceType;
            Amount = amount;
        }
    }
}