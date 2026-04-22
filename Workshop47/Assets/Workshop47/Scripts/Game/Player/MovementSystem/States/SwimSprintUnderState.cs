using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States
{
    public class SwimSprintUnderState : MovementBaseState
    {
        public SwimSprintUnderState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateContext.TargetHeight = stateContext.PlayerSettings.CrawlSettings.CrawlingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.CrawlSettings.CrawlingSpeed;
            
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.SwimSettings.SprintSwimAnimationTrigger);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (stateContext.SwimHandler.IsInWaterSurface())
            {
                Fsm.SetState<SwimSurfaceState>();
                return;
            }

            if (!stateContext.SwimHandler.IsUnderWater())
            {
                if (!stateContext.CharacterController.isGrounded)
                {
                    Fsm.SetState<AirState>();
                    return;
                }
                
                bool isSprintAllowed = stateContext.PlayerSettings.SprintSettings.IsMechanicAllowed;
                bool isRunAllowed = stateContext.PlayerSettings.RunSettings.IsMechanicAllowed;
                bool isSprintPressed = stateContext.MovementInput.IsSprintButtonPressed;
                bool canSprint = stateContext.MovementHandler.CanSprint();
                bool isWalkAllowed = stateContext.PlayerSettings.WalkSettings.IsMechanicAllowed;
                bool isWalkPressed = stateContext.MovementInput.IsWalkButtonPressed;
                
                if (!isWalkAllowed && !isSprintAllowed && !isRunAllowed)
                {
                    Fsm.SetState<IdleState>();
                    Debug.LogWarning("No movement mechanics are enabled!");
                    return;
                }
                
                if (isWalkAllowed && isWalkPressed || (!isSprintAllowed && !isRunAllowed))
                {
                    Fsm.SetState<WalkState>();
                    return;
                }
                
                if (isSprintAllowed && !isRunAllowed)
                {
                    Fsm.SetState<SprintState>();
                    return;
                }

                if (isSprintAllowed)
                {
                    if (isSprintPressed && canSprint)
                    {
                        Fsm.SetState<SprintState>();
                    }
                    else
                    {
                        Fsm.SetState<RunState>();
                    }
                    return;
                }
                
                Fsm.SetState<RunState>();
                return;
            }
            else
            {
                if (!stateContext.MovementInput.IsSprintButtonPressed 
                    || !stateContext.MovementHandler.CanSprint())
                {
                    Fsm.SetState<SwimUnderState>();
                    return;
                }
            }
        }

        protected override bool IsJumpingAllowed()
        {
            return false;
        }

        protected override void HandleMovement()
        {
            Vector3 swimDirection = stateContext.SwimHandler.GetSwimDirection();
            
            stateContext.DesiredVelocity += 
                swimDirection * (stateContext.PlayerSettings.SwimSettings.SprintSwimSpeed * Time.deltaTime);
            
            stateContext.DesiredVelocity -= stateContext.PlayerSettings.GeneralSettings.Friction / 2f * Time.deltaTime * 
                                      stateContext.DesiredVelocity;
            
            stateContext.DesiredVelocity = Vector3.ClampMagnitude(stateContext.DesiredVelocity, 
                stateContext.PlayerSettings.SwimSettings.SprintSwimSpeed);
        }
    }
}