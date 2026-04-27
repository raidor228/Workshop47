using Workshop47.Scripts.Game.State.Commands;
using Workshop47.Scripts.Game.State.GameResources;

namespace Workshop47.Scripts.Game.Gameplay.Commands
{
    public class CmdResourcesAdd : ICommand
    {
        public readonly ResourceType ResourceType;
        public readonly int Amount;
        
        public CmdResourcesAdd(ResourceType resourceType, int amount)
        {
            ResourceType = resourceType;
            Amount = amount;
        }
    }
}