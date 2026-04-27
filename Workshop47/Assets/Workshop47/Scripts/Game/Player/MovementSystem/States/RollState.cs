using Melador.Utils;
using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;
using Workshop47.Scripts.Utils;

namespace Melador.PlayerController.MovementController.States
{
    public class RollState : MovementBaseState
    {
        private float _rollTime;
        
        public RollState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            stateContext.TargetHeight = stateContext.PlayerSettings.RollSettings.RollingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.RollSettings.RollingSpeed;

            if (Fsm.PreviousState is IdleState)
            {
                Vector3 forward = stateContext.PlayerTransform.forward;
                stateContext.RollHandler.RollDirection = forward;
            }
            else
            {
                stateContext.RollHandler.RollDirection = stateContext.GetMoveDirection();
            }
            
            _rollTime = stateContext.PlayerSettings.RollSettings.RollingDuration;
            
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.RollSettings.RollAnimationTrigger);

        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _rollTime -= Time.deltaTime;

            if (!stateContext.CharacterController.isGrounded)
            {
                Fsm.SetState<AirState>();
                return;
            }
            
            RaycastHit closestHit =
                stateContext.GetClosestHitAround(stateContext.PlayerSettings.GeneralSettings.WallBounceLayer);
            if (_rollTime <= 0f 
                || (closestHit.distance != 0f && closestHit.distance <= 0.3f))
            {
                if (!stateContext.CharacterController.isGrounded)
                {
                    Fsm.SetState<AirState>();
                    return;
                }
                else
                {
                    if (stateContext.PlayerSettings.CrouchSettings.IsMechanicAllowed 
                        && !stateContext.CrouchHandler.CanStandUp())
                    {
                        Fsm.SetState<CrouchState>();
                        return;
                    }

                    if (stateContext.PlayerSettings.CrawlSettings.IsMechanicAllowed 
                        && !stateContext.CrawlHandler.CanStandUp())
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

                    if (stateContext.PlayerSettings.SlideSettings.IsMechanicAllowed 
                        && stateContext.SlopeHandler.IsOnSteepSlope())
                    {
                        Fsm.SetState<SlideState>();
                        return;
                    }

                    if (stateContext.PlayerSettings.ClimbSettings.IsMechanicAllowed 
                        && stateContext.ClimbHandler.CanClimbLadder())
                    {
                        Fsm.SetState<ClimbState>();
                        return;
                    }

                    if (stateContext.GetMoveDirection() == Vector3.zero)
                    {
                        Fsm.SetState<IdleState>();
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

        public override void Exit()
        {
            stateContext.RollHandler.CanRoll = false;
            Coroutines.Invoke(() => { stateContext.RollHandler.CanRoll = true; }, 
                stateContext.PlayerSettings.RollSettings.RollingCooldown);
        }

        protected override bool IsJumpingAllowed()
        {
            return false;
        }

        protected override void HandleMovement()
        {
            stateContext.DesiredVelocity = stateContext.RollHandler.RollDirection * 
                stateContext.PlayerSettings.RollSettings.RollingForce + Vector3.down;
        }
    }
}