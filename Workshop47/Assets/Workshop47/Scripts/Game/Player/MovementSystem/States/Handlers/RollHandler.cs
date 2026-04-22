using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States.Handlers
{
    public class RollHandler
    {
        public bool CanRoll { get; set; } = true;
        public Vector3 RollDirection { get; set; }

        private RollSettings RollSettings => _stateContext.PlayerSettings.RollSettings;
        
        private readonly MovementStateContext _stateContext;

        public RollHandler(MovementStateContext stateContext)
        {
            _stateContext = stateContext;
        }
    }
}