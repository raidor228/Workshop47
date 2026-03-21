using System.Collections.Generic;
using Workshop47.Scripts.Game.State.GameResources;

namespace Workshop47.Scripts.Game.State
{
    public class GameStateData
    {
        public int GlobalEntityId { get; set; }
        public List<ResourceData> Resources { get; set; }

        public int CreateEntityId()
        {
            return GlobalEntityId++;
        }
    }
}