using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States
{
    public class IdleState : MovementBaseState
    {
        public IdleState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateContext.TargetHeight = stateContext.PlayerSettings.GeneralSettings.StandingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.CrouchSettings.CrouchingSpeed;
            
            stateContext.MovementInput.OnRollButtonPressed += OnRoll;
            
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.GeneralSettings.IdleAnimationTrigger);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

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

            if (stateContext.CharacterController.isGrounded && stateContext.GetMoveDirection() != Vector3.zero)
            {
                bool isCrouchAllowed = stateContext.PlayerSettings.CrouchSettings.IsMechanicAllowed;
                bool canUncrouch = stateContext.CrouchHandler.CanStandUp();
                bool isSprintAllowed = stateContext.PlayerSettings.SprintSettings.IsMechanicAllowed;
                bool isWalkAllowed = stateContext.PlayerSettings.WalkSettings.IsMechanicAllowed;
                bool isRunAllowed = stateContext.PlayerSettings.RunSettings.IsMechanicAllowed;
                bool isWalkPressed = stateContext.MovementInput.IsWalkButtonPressed;
                bool isSprintPressed = stateContext.MovementInput.IsSprintButtonPressed;
                bool canSprint = stateContext.MovementHandler.CanSprint();

                if (isCrouchAllowed && !canUncrouch)
                {
                    Fsm.SetState<CrouchState>();
                    return;
                }

                if (!isWalkAllowed && !isSprintAllowed && !isRunAllowed)
                {
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

            if (!stateContext.CharacterController.isGrounded)
            {
                Fsm.SetState<AirState>();
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
            var velocity = Vector3.zero;
            velocity = Vector3.ClampMagnitude(velocity, stateContext.PlayerSettings.GeneralSettings.MaxVelocity);
            velocity.y = stateContext.MovementHandler.ApplyGravity(velocity.y, 
                stateContext.PlayerSettings.GeneralSettings.FallFactor);

            stateContext.DesiredVelocity = velocity;
        }

        private void OnRoll()
        {
            if (stateContext.PlayerSettings.RollSettings.IsMechanicAllowed
                && stateContext.RollHandler.CanRoll)
            {
                Fsm.SetState<RollState>();
                return;
            }
        }
    }
}