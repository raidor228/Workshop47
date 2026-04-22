using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States
{
    public class CrawlState : MovementBaseState
    {
        public CrawlState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateContext.TargetHeight = stateContext.PlayerSettings.CrawlSettings.CrawlingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.CrawlSettings.CrawlingSpeed;
            
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.CrawlSettings.CrawlAnimationTrigger);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!stateContext.CharacterController.isGrounded)
            {
                Fsm.SetState<AirState>();
                return;
            }
            
            if (stateContext.PlayerSettings.CrouchSettings.IsMechanicAllowed
                && stateContext.MovementInput.CrouchAction.WasPerformedThisFrame()
                && stateContext.CrawlHandler.CanCrouchFromCrawl())
            {
                Fsm.SetState<CrouchState>();
                return;
            }
            
            if (!stateContext.CrawlHandler.CanStandUp())
            {
                return;
            }
            
            if (stateContext.MovementInput.CrawlAction.WasPerformedThisFrame())
            {
                if (stateContext.GetMoveDirection() != Vector3.zero)
                {
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
                    Fsm.SetState<IdleState>();
                    return;
                }
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
                           (stateContext.PlayerSettings.CrawlSettings.CrawlAcceleration * Time.deltaTime);

            velocity = stateContext.MovementHandler.ApplyFrictionToVelocity(velocity,
                stateContext.PlayerSettings.GeneralSettings.Friction);
            velocity = Vector3.ClampMagnitude(velocity, stateContext.PlayerSettings.GeneralSettings.MaxVelocity);
            velocity.y = stateContext.MovementHandler.ApplyGravity(velocity.y, 
                stateContext.PlayerSettings.GeneralSettings.FallFactor);
            
            stateContext.DesiredVelocity = velocity;
        }
    }
}