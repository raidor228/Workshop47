using System;
using System.Collections.Generic;
using Workshop47.Scripts.Game.Settings.Settlement.Entities;

namespace Workshop47.Scripts.Game.Settings.Settlement.Maps
{
    [Serializable]
    public class MapInitialStateSettings
    {
        public List<EntityInitialStateSettings> Entities;
    }
}