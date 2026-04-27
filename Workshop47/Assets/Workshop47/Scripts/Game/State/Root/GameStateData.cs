using System.Collections.Generic;
using Workshop47.Scripts.Game.State.Entities;
using Workshop47.Scripts.Game.State.Entities.Upgradeable.Player;
using Workshop47.Scripts.Game.State.GameResources;
using Workshop47.Scripts.Game.State.Maps;

namespace Workshop47.Scripts.Game.State.Root
{
    public class GameStateData
    {
        public int GlobalEntityId { get; set; }
        public int CurrentMapId { get; set; }
        public int ControllableEntityId { get; set; }
        public List<ResourceData> Resources { get; set; }
        public List<MapData> Maps { get; set; }
        public PlayerEntityData Player { get; set; }
        
        public int CreateEntityId()
        {
            return GlobalEntityId++;
        }
    }
}