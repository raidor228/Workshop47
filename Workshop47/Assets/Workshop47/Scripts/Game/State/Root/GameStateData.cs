using System.Collections.Generic;
using Workshop47.Scripts.Game.State.Entities;
using Workshop47.Scripts.Game.State.GameResources;

namespace Workshop47.Scripts.Game.State.Root
{
    public class GameStateData
    {
        public int GlobalEntityId { get; set; }
        public List<ResourceData> Resources { get; set; }
        public List<EntityData> Entities { get; set; }

        public int CreateEntityId()
        {
            return GlobalEntityId++;
        }
    }
}