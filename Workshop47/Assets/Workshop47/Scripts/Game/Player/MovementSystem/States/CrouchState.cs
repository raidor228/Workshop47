using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States
{
    public class CrouchState : MovementBaseState
    {
        public CrouchState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateContext.TargetHeight = stateContext.PlayerSettings.CrouchSettings.CrouchingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.CrouchSettings.CrouchingSpeed;
            
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.CrouchSettings.CrouchAnimationTrigger);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (stateContext.PlayerSettings.CrawlSettings.IsMechanicAllowed
                && stateContext.MovementInput.CrawlAction.WasPerformedThisFrame())
            {
                Fsm.SetState<CrawlState>();
                return;
            }

            if (stateContext.PlayerSettings.SwimSettings.IsMechanicAllowed)
            {
                if (stateContext.SwimHandler.IsInWaterSurface())
                {
                    Fsm.SetState<SwimSurfaceState>();
                    return;
                }
                else if (stateContext.SwimHandler.IsUnderWater())
                {
                    Fsm.SetState<SwimUnderState>();
                    return;
                }
            }

            if (!stateContext.CharacterController.isGrounded)
            {
                Fsm.SetState<AirState>();
                return;
            }
            else if (stateContext.CrouchHandler.CanStandUp())
            {
                if (stateContext.GetMoveDirection() == Vector3.zero
                    && !stateContext.MovementInput.IsCrouchButtonPressed)
                {
                    Fsm.SetState<IdleState>();
                    return;
                }
                
                bool isCrouchPressed = stateContext.MovementInput.IsCrouchButtonPressed;
                bool isSprintAllowed = stateContext.PlayerSettings.SprintSettings.IsMechanicAllowed;
                bool isRunAllowed = stateContext.PlayerSettings.RunSettings.IsMechanicAllowed;
                bool isSprintPressed = stateContext.MovementInput.IsSprintButtonPressed;
                bool canSprint = stateContext.MovementHandler.CanSprint();
                bool isWalkAllowed = stateContext.PlayerSettings.WalkSettings.IsMechanicAllowed;
                bool isWalkPressed = stateContext.MovementInput.IsWalkButtonPressed;
                
                if (!isCrouchPressed)
                {
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
            var moveDirection = stateContext.GetMoveDirection();
            var adjustedVelocityToSlope = stateContext.MovementHandler.AdjustVelocityToSlope(
                stateContext.PlayerTransform.position, moveDirection);
            
            var velocity = stateContext.HorizontalDesiredVelocity + 
                           adjustedVelocityToSlope * 
                           (stateContext.PlayerSettings.CrouchSettings.CrouchAcceleration * Time.deltaTime);

            velocity = stateContext.MovementHandler.ApplyFrictionToVelocity(velocity,
                stateContext.PlayerSettings.GeneralSettings.Friction);
            velocity = Vector3.ClampMagnitude(velocity, stateContext.PlayerSettings.GeneralSettings.MaxVelocity);
            velocity.y = stateContext.MovementHandler.ApplyGravity(velocity.y, 
                stateContext.PlayerSettings.GeneralSettings.FallFactor);
            
            stateContext.DesiredVelocity = velocity;
        }
    }
}