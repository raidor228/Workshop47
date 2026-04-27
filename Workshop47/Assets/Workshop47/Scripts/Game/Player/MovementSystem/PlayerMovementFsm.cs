using Melador.PlayerController.MovementController.States;
using Melador.PlayerController.MovementController.States.Root;
using Workshop47.Scripts.Fsm;

namespace Melador.PlayerController.MovementController
{
    public class PlayerMovementFsm : Fsm
    {
        public readonly Workshop47.Scripts.Game.Player.PlayerController Controller;
        public float VelocitySqrMagnitude => _stateContext.Velocity.sqrMagnitude;
        
        private readonly MovementStateContext _stateContext;
        
        public PlayerMovementFsm(Workshop47.Scripts.Game.Player.PlayerController controller)
        {
            Controller = controller;

            _stateContext = new MovementStateContext(this)
            {
                TargetHeight = controller.PlayerSettings.GeneralSettings.StandingHeight
            };
            
            AddState(new AirState(_stateContext, this));
            AddState(new ClimbState(_stateContext, this));
            AddState(new CrouchState(_stateContext, this));
            AddState(new CrawlState(_stateContext, this));
            AddState(new IdleState(_stateContext, this));
            AddState(new LedgeGrabState(_stateContext, this));
            AddState(new SlideState(_stateContext, this));
            AddState(new SwimSurfaceState(_stateContext, this));
            AddState(new SwimUnderState(_stateContext, this));
            AddState(new SwimSprintUnderState(_stateContext, this));
            AddState(new VaultState(_stateContext, this));
            AddState(new WalkState(_stateContext, this));
            AddState(new RunState(_stateContext, this));
            AddState(new SprintState(_stateContext, this));
            AddState(new WallRunState(_stateContext, this));
            AddState(new RollState(_stateContext, this));
            
            SetState<AirState>();
        }
    }
}