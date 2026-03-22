using System.Collections.Generic;
using Workshop47.Scripts.Game.State.Entities;

namespace Workshop47.Scripts.Game.State.Maps
{
    public class MapData
    {
        public int Id { get; set; }
        public List<EntityData> Entities { get; set; }
    }
}