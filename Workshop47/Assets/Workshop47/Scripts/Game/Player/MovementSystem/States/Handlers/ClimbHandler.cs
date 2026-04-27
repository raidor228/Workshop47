using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States.Handlers
{
    public class ClimbHandler
    {
        public Vector3 ClimbDirection { get; set; }
        public Collider LadderCollider { get; set; }
        public bool HasLadderCollision { get; set; }
        
        private ClimbSettings ClimbSettings => _stateContext.PlayerSettings.ClimbSettings;
        
        private readonly MovementStateContext _stateContext;
        
        public ClimbHandler(MovementStateContext stateContext)
        {
            _stateContext = stateContext;
        }
        
        public bool CanClimbLadder()
        {
            return HasLadderCollision;
        }
    }
}