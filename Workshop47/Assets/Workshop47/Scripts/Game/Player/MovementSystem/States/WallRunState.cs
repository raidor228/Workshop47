using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States
{
    public class WallRunState : MovementBaseState
    {
        private float _wallGravity;
        private float _timeOnWall;
        private float _wallRunTime;

        public WallRunState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateContext.WallRunHandler.CurrentWallNormal = stateContext.WallRunHandler.GetWallNormal();
            stateContext.WallRunHandler.WallDirection = stateContext.WallRunHandler.GetHorizontalParallel();
            
            _timeOnWall = 0f;
            _wallRunTime = 0f;
            
            stateContext.WallRunHandler.CanWallJump = true;
            stateContext.TargetHeight = stateContext.PlayerSettings.GeneralSettings.StandingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.CrouchSettings.CrouchingSpeed;

            Vector3 velocity = stateContext.DesiredVelocity;
            velocity.y = 0;
            stateContext.DesiredVelocity = velocity;
            
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.WallRunSettings.WallRunAnimationTrigger);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            Vector3 wallNormal = stateContext.WallRunHandler.GetWallNormal();
            if (stateContext.WallRunHandler.CurrentWallNormal != wallNormal)
            {
                if (Vector3.Angle(wallNormal, stateContext.WallRunHandler.CurrentWallNormal) > 45f)
                {
                    Fsm.SetState<AirState>();
                    return;
                }
            }

            _timeOnWall += Time.deltaTime;
            _wallRunTime += Time.deltaTime;

            if (_timeOnWall >= stateContext.PlayerSettings.WallRunSettings.WallDuration)
            {
                Vector3 velocity = stateContext.DesiredVelocity;
                velocity.y = Mathf.Max(-2f, stateContext.DesiredVelocity.y);
                stateContext.DesiredVelocity = velocity;
                
                Fsm.SetState<AirState>();
                return;
            }

            if (stateContext.MovementInput.MovementInput.y <= 0)
            {
                Fsm.SetState<AirState>();
                return;
            }
            
            if (_wallRunTime >= stateContext.PlayerSettings.WallRunSettings.WallKickWindow)
            {
                float desiredDirectionAngle = stateContext.WallRunHandler.GetDesiredWallNormalAngle();
                if (desiredDirectionAngle > 0f && desiredDirectionAngle < 44f)
                {
                    stateContext.DesiredVelocity += stateContext.WallRunHandler.CurrentWallNormal * 5f;
                    Fsm.SetState<AirState>();
                    return;
                }
            }
            
            if (stateContext.PlayerSettings.SlideSettings.IsMechanicAllowed 
                && stateContext.SlopeHandler.IsOnSteepSlope() 
                && stateContext.MovementInput.IsSlideButtonPressed)
            {
                Fsm.SetState<SlideState>();
                return;
            }

            if (stateContext.IsGrounded())
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
                if (stateContext.MovementInput.IsCrouchButtonPressed 
                    || !stateContext.WallRunHandler.ShouldMaintainWallRun())
                {
                    Fsm.SetState<AirState>();
                    return;
                }
            }
        }

        public override void Exit()
        {
            stateContext.WallRunHandler.OffWallTimeStamp = Time.time;
            stateContext.WallRunHandler.PreviousWallNormal = stateContext.WallRunHandler.CurrentWallNormal;
            
            Vector3 velocity = stateContext.DesiredVelocity;
            velocity.y = Mathf.Max(-2f, stateContext.DesiredVelocity.y);
            stateContext.DesiredVelocity = velocity;
        }
        
        protected override bool IsJumpingAllowed()
        {
            return !stateContext.SlopeHandler.IsOnSteepSlope();
        }

        protected override void HandleJumping()
        {
            if (stateContext.MovementInput.IsJumpButtonPressed 
                && stateContext.WallRunHandler.CanWallJump
                && !stateContext.CharacterController.isGrounded 
                && !stateContext.MovementInput.IsCrouchButtonPressed)
            {
                PerformJumpOfWall();
            }
        }

        protected override void HandleMovement()
        {
            float desiredDirectionAngle = stateContext.WallRunHandler.GetDesiredWallNormalAngle();
            if (desiredDirectionAngle <= 0f || desiredDirectionAngle >= 44f)
            {
                Vector3 pullDirection = stateContext.WallRunHandler.WallDirection;
                if (Vector3.Angle(stateContext.DesiredVelocity, pullDirection) > 
                    Vector3.Angle(stateContext.DesiredVelocity, -pullDirection))
                {
                    pullDirection = -pullDirection;
                }

                var velocity = stateContext.DesiredVelocity;
                
                velocity +=
                    pullDirection * (stateContext.PlayerSettings.WallRunSettings.WallAcceleration * Time.deltaTime);
                velocity = stateContext.MovementHandler.ApplyFrictionToVelocity(velocity,
                    stateContext.PlayerSettings.GeneralSettings.Friction);

                stateContext.DesiredVelocity = velocity;
            }
        }
        
        private void PerformJumpOfWall()
        {
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.WallRunSettings.WallRunJumpAnimationTrigger);
            
            stateContext.DesiredVelocity = stateContext.WallRunHandler.GetJumpDirection();

            stateContext.DesiredVelocity = new Vector3(stateContext.DesiredVelocity.x,
                Mathf.Sqrt(-2f * stateContext.PlayerSettings.GeneralSettings.Gravity), 
                stateContext.DesiredVelocity.z);
            
            stateContext.HasJumped = true;
            stateContext.WallRunHandler.CanWallJump = false;
        }
    }
}