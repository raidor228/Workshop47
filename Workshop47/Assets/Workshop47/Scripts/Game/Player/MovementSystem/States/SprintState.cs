using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States
{
    public class SprintState : MovementBaseState
    {
        private float _longJumpTime;
        
        public SprintState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateContext.TargetHeight = stateContext.PlayerSettings.GeneralSettings.StandingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.CrouchSettings.CrouchingSpeed;
            stateContext.WallRunHandler.CanWallJump = false;
            
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.SprintSettings.SprintAnimationTrigger);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (_longJumpTime >= 0f)
            {
                _longJumpTime -= Time.deltaTime;
            }
            
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
                bool isRunAllowed = stateContext.PlayerSettings.RunSettings.IsMechanicAllowed;
                bool isSprintPressed = stateContext.MovementInput.IsSprintButtonPressed;
                bool canSprint = stateContext.MovementHandler.CanSprint();
                bool isWalkAllowed = stateContext.PlayerSettings.WalkSettings.IsMechanicAllowed;
                bool isWalkPressed = stateContext.MovementInput.IsWalkButtonPressed;

                if (isWalkAllowed && isWalkPressed)
                {
                    Fsm.SetState<WalkState>();
                    return;
                }
                
                if ((!canSprint || !isSprintPressed) && isRunAllowed)
                {
                    Fsm.SetState<RunState>();
                    return;
                }

                RaycastHit closestHit =
                    stateContext.GetClosestHitAround(stateContext.PlayerSettings.GeneralSettings.WallBounceLayer);
                if (closestHit.distance <= 0.3f && isRunAllowed)
                {
                    Fsm.SetState<RunState>();
                    return;
                }
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

            if ((stateContext.PlayerSettings.SlideSettings.IsMechanicAllowed 
                 && stateContext.MovementInput.IsSlideButtonPressed 
                 && stateContext.HorizontalDesiredVelocity.magnitude > stateContext.PlayerSettings.SlideSettings.SlideVelocityThreshold)
                 || stateContext.SlopeHandler.IsOnSteepSlope())
            {
                Fsm.SetState<SlideState>();
                return;
            }

            if (!stateContext.PlayerSettings.SlideSettings.IsMechanicAllowed
                && stateContext.MovementInput.IsCrouchButtonPressed)
            {
                Fsm.SetState<CrouchState>();
                return;
            }
        }

        protected override void HandleJumping()
        {
            if (!stateContext.CharacterController.isGrounded)
            {
                return;
            }
            
            if (stateContext.PlayerSettings.LongJumpSettings.IsMechanicAllowed
                && stateContext.MovementInput.IsJumpButtonPressed
                && stateContext.HorizontalDesiredVelocity.magnitude >=
                stateContext.PlayerSettings.LongJumpSettings.MinVelocityToLongJump
                && _longJumpTime <= 0f)
            {
                PerformLongJump();
                _longJumpTime = stateContext.PlayerSettings.LongJumpSettings.LongJumpCooldown;
            }
            else
            {
                if (stateContext.PlayerSettings.JumpSettings.AutoBhop 
                    && stateContext.MovementInput.IsJumpButtonPressed)
                {
                    PerformManualJump();
                }
                else if (stateContext.MovementInput.JumpAction.WasPerformedThisFrame())
                {
                    PerformManualJump();
                }
            }
        }
        
        private void PerformLongJump()
        {
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.LongJumpSettings.LongJumpAnimationTrigger);
            
            stateContext.HasJumped = true;
            
            float jumpVelocity = Mathf.Sqrt(-2f * stateContext.PlayerSettings.GeneralSettings.Gravity * 
                                            stateContext.PlayerSettings.LongJumpSettings.LongJumpHeight);
            
            stateContext.DesiredVelocity = new Vector3(stateContext.DesiredVelocity.x, jumpVelocity, stateContext.DesiredVelocity.z);
            
            stateContext.DesiredVelocity += stateContext.GetMoveDirection() * 
                                     (stateContext.PlayerSettings.LongJumpSettings.LongJumpForce * Time.deltaTime);
        }

        protected override void HandleMovement()
        {
            var moveDirection = stateContext.GetMoveDirection();
            var adjustedVelocityToSlope = stateContext.MovementHandler.AdjustVelocityToSlope(
                stateContext.PlayerTransform.position, moveDirection);
            
            var velocity = stateContext.HorizontalDesiredVelocity + 
                           adjustedVelocityToSlope * 
                           (stateContext.PlayerSettings.SprintSettings.SprintAcceleration * Time.deltaTime);
            
            velocity = stateContext.MovementHandler.ApplyFrictionToVelocity(velocity,
                stateContext.PlayerSettings.GeneralSettings.Friction);
            velocity = Vector3.ClampMagnitude(velocity, stateContext.PlayerSettings.GeneralSettings.MaxVelocity);
            velocity.y = stateContext.MovementHandler.ApplyGravity(velocity.y, 
                stateContext.PlayerSettings.GeneralSettings.FallFactor);
            
            stateContext.DesiredVelocity = velocity;
        }
    }
}