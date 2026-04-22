using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States
{
    public class SwimSurfaceState : MovementBaseState
    {
        public SwimSurfaceState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateContext.SwimHandler.ShouldWaterJump = false;
            stateContext.TargetHeight = stateContext.PlayerSettings.GeneralSettings.StandingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.CrouchSettings.CrouchingSpeed;

            if (Fsm.PreviousState is not SwimUnderState)
            {
                stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.SwimSettings.SwimAnimationTrigger);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            stateContext.SwimHandler.HandleSurfaceWaterJump();

            stateContext.DesiredVelocity -= stateContext.PlayerSettings.GeneralSettings.Friction / 2f * 
                                                     Time.deltaTime * stateContext.DesiredVelocity;
            stateContext.DesiredVelocity = Vector3.ClampMagnitude(stateContext.DesiredVelocity, 
                stateContext.PlayerSettings.SwimSettings.SwimSpeed);
            
            if (stateContext.SwimHandler.IsUnderWater())
            {
                Fsm.SetState<SwimUnderState>();
                return;
            }
            else
            {
                if (!stateContext.SwimHandler.IsSwimming)
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
            }
        }
        
        protected override bool IsJumpingAllowed()
        {
            return false;
        }

        protected override void HandleMovement()
        {
            Vector3 swimDirection = stateContext.SwimHandler.GetSwimDirection();
            if (stateContext.GetCameraForward().y > 
                stateContext.PlayerSettings.SwimSettings.SwimDownThreshold)
            {
                swimDirection = stateContext.GetMoveDirection();
            }

            stateContext.DesiredVelocity += 
                swimDirection * (stateContext.PlayerSettings.SwimSettings.SwimSpeed * Time.deltaTime);
        }
    }
}