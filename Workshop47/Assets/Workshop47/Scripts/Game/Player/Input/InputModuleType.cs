using System;

namespace Melador.PlayerInput.Modules
{
    [Flags]
    public enum InputModuleType
    {
        None         = 0,
        Movement     = 1 << 1,
        Camera       = 1 << 2,
        Interactions = 1 << 3,
        All          = Movement | Camera | Interactions
    }
}