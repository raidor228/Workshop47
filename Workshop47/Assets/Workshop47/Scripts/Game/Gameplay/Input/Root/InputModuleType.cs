using System;

namespace Workshop47.Scripts.Game.Gameplay.Input
{
    [Flags]
    public enum InputModuleType
    {
        None         = 0,
        Movement     = 1 << 1,
        Camera       = 1 << 2,
        Interactions = 1 << 3,
        RtsCamera    = 1 << 4,
        Player       = Movement | Camera | Interactions,
        Rts          = RtsCamera,
        All          = Player | Rts
    }
}