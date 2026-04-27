using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States
{
    public class WalkState : MovementBaseState
    {
        public WalkState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateContext.TargetHeight = stateContext.PlayerSettings.GeneralSettings.StandingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.CrouchSettings.CrouchingSpeed;
            stateContext.WallRunHandler.CanWallJump = false;
            
            stateContext.MovementInput.OnRollButtonPressed += OnRoll;
            
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.WalkSettings.WalkAnimationTrigger);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (!stateContext.CharacterController.isGrounded)
            {
                Fsm.SetState<AirState>();
                return;
            }
            
            bool canUncrouch = stateContext.CrouchHandler.CanStandUp();
            bool isCrouchAllowed = stateContext.PlayerSettings.CrouchSettings.IsMechanicAllowed;
            if (stateContext.GetMoveDirection() == Vector3.zero)
            {
                if (isCrouchAllowed && canUncrouch)
                {
                    Fsm.SetState<IdleState>();
                    return;
                }
            }
            else
            {
                bool isSprintAllowed = stateContext.PlayerSettings.SprintSettings.IsMechanicAllowed;
                bool isRunAllowed = stateContext.PlayerSettings.RunSettings.IsMechanicAllowed;
                bool isSprintPressed = stateContext.MovementInput.IsSprintButtonPressed;
                bool canSprint = stateContext.MovementHandler.CanSprint();
                bool isWalkPressed = stateContext.MovementInput.IsWalkButtonPressed;

                if (!isWalkPressed)
                {
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

                    if (isRunAllowed)
                    {
                        Fsm.SetState<RunState>();
                        return;
                    }
                }
            }
            
            if (stateContext.PlayerSettings.CrouchSettings.IsMechanicAllowed 
                && stateContext.MovementInput.IsCrouchButtonPressed)
            {
                Fsm.SetState<CrouchState>();
                return;
            }

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
            
            if (stateContext.PlayerSettings.ClimbSettings.IsMechanicAllowed 
                && stateContext.ClimbHandler.CanClimbLadder())
            {
                Fsm.SetState<ClimbState>();
                return;
            }
            
            if (stateContext.PlayerSettings.SlideSettings.IsMechanicAllowed 
                && stateContext.SlopeHandler.IsOnSteepSlope())
            {
                Fsm.SetState<SlideState>();
                return;
            }
        }

        public override void Exit()
        {
            stateContext.MovementInput.OnRollButtonPressed -= OnRoll;
        }

        protected override bool IsJumpingAllowed()
        {
            return true;
        }

        protected override void HandleMovement()
        {
            var moveDirection = stateContext.GetMoveDirection();
            var adjustedVelocityToSlope = stateContext.MovementHandler.AdjustVelocityToSlope(
                stateContext.PlayerTransform.position, moveDirection);
            
            var velocity = stateContext.HorizontalDesiredVelocity + 
                           adjustedVelocityToSlope * 
                           (stateContext.PlayerSettings.WalkSettings.WalkAcceleration * Time.deltaTime);

            velocity = stateContext.MovementHandler.ApplyFrictionToVelocity(velocity,
                stateContext.PlayerSettings.GeneralSettings.Friction);
            velocity = Vector3.ClampMagnitude(velocity, stateContext.PlayerSettings.GeneralSettings.MaxVelocity);
            velocity.y = stateContext.MovementHandler.ApplyGravity(velocity.y, 
                stateContext.PlayerSettings.GeneralSettings.FallFactor);
            
            stateContext.DesiredVelocity = velocity;
        }

        private void OnRoll()
        {
            if (stateContext.RollHandler.CanRoll)
            {
                Fsm.SetState<RollState>();
                return;
            }
        }
    }
}