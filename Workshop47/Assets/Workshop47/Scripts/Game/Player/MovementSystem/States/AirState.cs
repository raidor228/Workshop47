using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States
{
    public class AirState : MovementBaseState
    {
        private float _inAirTime;

        public AirState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _inAirTime = 0f;
            stateContext.TargetHeight = stateContext.PlayerSettings.GeneralSettings.StandingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.CrouchSettings.CrouchingSpeed;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            HandleCoyoteTime();

            if (stateContext.PlayerSettings.ClimbSettings.IsMechanicAllowed 
                && stateContext.ClimbHandler.CanClimbLadder())
            {
                Fsm.SetState<ClimbState>();
                return;
            }
            
            if (stateContext.PlayerSettings.LedgeSettings.IsMechanicAllowed 
                && stateContext.LedgeHandler.CanGrabLedge())
            {
                Fsm.SetState<LedgeGrabState>();
                return;
            }
            
            if (stateContext.PlayerSettings.VaultSettings.IsMechanicAllowed 
                && stateContext.VaultHandler.CanVault() 
                && stateContext.MovementInput.IsJumpButtonPressed)
            {
                Fsm.SetState<VaultState>();
                return;
            }

            if (!stateContext.CharacterController.isGrounded)
            {
                if (stateContext.PlayerSettings.WallRunSettings.IsMechanicAllowed
                    && !stateContext.MovementInput.IsCrouchButtonPressed 
                    && IsWallValid(stateContext.WallRunHandler.PreviousWallNormal)
                    && stateContext.DesiredVelocity.y >= stateContext.PlayerSettings.WallRunSettings.MinVerticalVelocity)
                {
                    Fsm.SetState<WallRunState>();
                    return;
                }
                else if (stateContext.PlayerSettings.SlideSettings.IsMechanicAllowed 
                         && stateContext.SlopeHandler.IsOnSteepSlope() 
                         && !stateContext.WallRunHandler.IsWallBeneath())
                {
                    Fsm.SetState<SlideState>();
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
            }
            
            if (stateContext.CharacterController.isGrounded)
            {
                bool canUncrouch = stateContext.CrouchHandler.CanStandUp();
                bool isSprintAllowed = stateContext.PlayerSettings.SprintSettings.IsMechanicAllowed;
                bool isRunAllowed = stateContext.PlayerSettings.RunSettings.IsMechanicAllowed;
                bool isSprintPressed = stateContext.MovementInput.IsSprintButtonPressed;
                bool canSprint = stateContext.MovementHandler.CanSprint();
                bool isWalkAllowed = stateContext.PlayerSettings.WalkSettings.IsMechanicAllowed;
                bool isWalkPressed = stateContext.MovementInput.IsWalkButtonPressed;
                
                if (stateContext.GetMoveDirection() == Vector3.zero && canUncrouch)
                {
                    Fsm.SetState<IdleState>();
                    return;
                }
                else
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

        public override void Exit()
        {
            stateContext.HasJumped = false;
        }

        protected override void HandleMovement()
        {
            var moveDirection = stateContext.GetMoveDirection();
            var adjustedVelocityToSlope = stateContext.MovementHandler.AdjustVelocityToSlope(
                stateContext.PlayerTransform.position, moveDirection);
            
            var velocity = stateContext.HorizontalDesiredVelocity + 
                           adjustedVelocityToSlope * 
                           (stateContext.PlayerSettings.GeneralSettings.AirAcceleration * Time.deltaTime);

            float y = stateContext.MovementHandler.GetVelocityAfterHeadBounce(stateContext.DesiredVelocity.y);
            y += stateContext.PlayerSettings.GeneralSettings.Gravity * Time.deltaTime;
            
            float lossFactor = 1f - stateContext.PlayerSettings.GeneralSettings.AirLossFactor / 10000f;
            velocity *= lossFactor;
            
            velocity = Vector3.ClampMagnitude(velocity, stateContext.PlayerSettings.GeneralSettings.MaxAirVelocity);
            velocity.y = Mathf.Max(y, stateContext.PlayerSettings.GeneralSettings.MinVerticalVelocity);

            stateContext.DesiredVelocity = velocity;
        }
        
        public override void OnControllerColliderHit(ControllerColliderHit hit)
        {
            base.OnControllerColliderHit(hit);

            switch (hit.gameObject.tag)
            {
                case TagsAndLayers.Obstacle:
                    stateContext.DesiredVelocity -=
                        stateContext.PlayerSettings.GeneralSettings.Friction * Time.deltaTime *
                        stateContext.HorizontalDesiredVelocity;
                    break;
            }
        }
        
        private void HandleCoyoteTime()
        {
            _inAirTime += Time.deltaTime;
            if (_inAirTime > stateContext.PlayerSettings.JumpSettings.CoyoteTime &&
                !stateContext.HasJumped)
            {
                stateContext.WallRunHandler.CanWallJump = false;
            }
        }

        private bool IsWallValid(Vector3 previousDir)
        {
            float timer = Time.time - stateContext.WallRunHandler.OffWallTimeStamp;
            float angle = Vector3.Angle(Vector3.up, stateContext.WallRunHandler.GetWallNormal());
            
            return timer >= stateContext.PlayerSettings.WallRunSettings.WallCooldown && angle > 45f &&
                   stateContext.WallRunHandler.GetDistanceToWall() <= 0.3f &&
                   (timer >= stateContext.PlayerSettings.WallRunSettings.SameWallCooldown ||
                    stateContext.WallRunHandler.GetWallNormal() != previousDir);
        }
    }
}