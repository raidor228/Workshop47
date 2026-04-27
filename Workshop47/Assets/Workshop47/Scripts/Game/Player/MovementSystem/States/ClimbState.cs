using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States
{
    public class ClimbState : MovementBaseState
    {
        private readonly float _climbDownThreshold = -0.4f;
        
        public ClimbState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateContext.TargetHeight = stateContext.PlayerSettings.GeneralSettings.StandingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.CrouchSettings.CrouchingSpeed;
            
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.ClimbSettings.ClimbAnimationTrigger);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (!stateContext.ClimbHandler.CanClimbLadder())
            {
                if (stateContext.CharacterController.isGrounded)
                {
                    if (stateContext.GetMoveDirection() == Vector3.zero)
                    {
                        Fsm.SetState<IdleState>();
                        return;
                    }
                    else
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
                }
                else
                {
                    Fsm.SetState<AirState>();
                    return;
                }
            }
        }

        protected override void HandleMovement()
        {
            Vector2 movementInput = stateContext.MovementInput.MovementInput;

            Vector3 climbDirection = stateContext.ClimbHandler.ClimbDirection.normalized * movementInput.y;
            climbDirection *= (stateContext.GetCameraForward().y <= _climbDownThreshold) ? -1 : 1;
            if (stateContext.MovementInput.IsJumpButtonPressed)
            {
                climbDirection.y = 1f;
            }
            else if (stateContext.MovementInput.IsCrouchButtonPressed)
            {
                climbDirection.y = -1f;
            }
            
            Transform ladder = stateContext.ClimbHandler.LadderCollider.transform;
            Vector3 ladderPosition = ladder.position;
            ladderPosition.y = stateContext.PlayerTransform.position.y;
            
            Vector3 toTarget = (ladderPosition - stateContext.PlayerTransform.position).normalized;
            Vector3 cameraForward = stateContext.GetCameraForward();

            Vector3 horizontalDirection = -Vector3.Cross(ladder.forward, ladder.right);
            
            float dot = Vector3.Dot(cameraForward, toTarget);
            if ((dot > 0 && movementInput.y < 0)
                || (dot < 0 && movementInput.y > 0))
            {
                climbDirection += horizontalDirection * 0.5f;
            }
            else
            {
                climbDirection += -horizontalDirection * 0.5f;
            }
            
            Vector3 climbVelocity = climbDirection * stateContext.PlayerSettings.ClimbSettings.ClimbUpSpeed;

            stateContext.DesiredVelocity = climbVelocity;
        }
    }
}