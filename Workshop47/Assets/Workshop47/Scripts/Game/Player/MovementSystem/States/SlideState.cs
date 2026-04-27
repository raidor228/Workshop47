using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States
{
    public class SlideState : MovementBaseState
    {
        private float _slideTimer;
        
        public SlideState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateContext.TargetHeight = stateContext.PlayerSettings.CrawlSettings.CrawlingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.CrawlSettings.CrawlingSpeed;
            
            _slideTimer = stateContext.PlayerSettings.SlideSettings.SlideTime;
            stateContext.SlopeHandler.SlideTransitionTimer = 0f;
            
            stateContext.SlopeHandler.ApplyBoostIfEligible();
            
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.SlideSettings.SlideAnimationTrigger);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _slideTimer -= Time.deltaTime;
            
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

            if (!stateContext.SlopeHandler.IsOnSlope() && !stateContext.CharacterController.isGrounded)
            {
                Fsm.SetState<AirState>();
                return;
            }
            
            RaycastHit closestHit =
                stateContext.GetClosestHitAround(stateContext.PlayerSettings.GeneralSettings.WallBounceLayer);
            if ((!stateContext.SlopeHandler.IsOnSlope() && _slideTimer < 0f)
                || (closestHit.distance <= 0.3f && closestHit.distance != 0f))
            {
                bool isCrouchAllowed = stateContext.PlayerSettings.CrouchSettings.IsMechanicAllowed;
                bool isCrouchPressed = stateContext.MovementInput.IsCrouchButtonPressed;
                bool isSprintAllowed = stateContext.PlayerSettings.SprintSettings.IsMechanicAllowed;
                bool isRunAllowed = stateContext.PlayerSettings.RunSettings.IsMechanicAllowed;
                bool isSprintPressed = stateContext.MovementInput.IsSprintButtonPressed;
                bool canSprint = stateContext.MovementHandler.CanSprint();
                bool isWalkAllowed = stateContext.PlayerSettings.WalkSettings.IsMechanicAllowed;
                bool isWalkPressed = stateContext.MovementInput.IsWalkButtonPressed;
                
                if (stateContext.GetMoveDirection() == Vector3.zero)
                {
                    Fsm.SetState<IdleState>();
                    return;
                }
                
                if (isCrouchAllowed && isCrouchPressed)
                {
                    Fsm.SetState<CrouchState>();
                    return;
                }

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
        
        public override void Exit()
        {
            if (stateContext.SlopeHandler.IsOnSlope())
            {
                stateContext.DesiredVelocity /= 2f;
            }
        }

        protected override bool IsJumpingAllowed()
        {
            return false;
        }

        protected override void HandleMovement()
        {
            var velocity = stateContext.HorizontalDesiredVelocity;
            
            Vector2 input = stateContext.MovementInput.MovementInput;
            Vector3 normalized = Vector3.Cross(Vector3.up, velocity.normalized).normalized;

            velocity += normalized * (input.x * stateContext.PlayerSettings.SlideSettings.SlidingSideAcceleration * 
                                      Time.deltaTime);
            
            velocity /= stateContext.PlayerSettings.SlideSettings.SlidingSlowdownFactor;
            velocity = Vector3.ClampMagnitude(velocity, stateContext.PlayerSettings.GeneralSettings.MaxVelocity);
            velocity.y = stateContext.MovementHandler.ApplyGravity(velocity.y, 
                stateContext.PlayerSettings.SlideSettings.FallFactor);
            
            stateContext.DesiredVelocity = velocity;
            
            if (stateContext.SlopeHandler.IsOnSlope())
            {
                _slideTimer = stateContext.PlayerSettings.SlideSettings.SlideBlendTime;
                stateContext.SlopeHandler.AdjustVelocityOnSlope();
            }
            else // flat surface
            {
                _slideTimer -= Time.deltaTime;
            }
        }
    }
}