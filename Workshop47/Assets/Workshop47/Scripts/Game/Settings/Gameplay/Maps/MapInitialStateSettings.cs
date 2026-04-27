using System;
using System.Collections.Generic;
using Workshop47.Scripts.Game.Settings.Gameplay.Entities;

namespace Workshop47.Scripts.Game.Settings.Gameplay.Maps
{
    [Serializable]
    public class MapInitialStateSettings
    {
        public List<EntityInitialStateSettings> Entities;
    }
}