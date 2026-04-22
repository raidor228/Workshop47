using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States
{
    public class VaultState : MovementBaseState
    {
        private Vector3 _startPosition;
        private Vector3 _previousPosition;

        private Vector3 _vaultHorizontalDirection;
        private float _startSpeed;
        private float _duration;
        private float _angle;
        private float _timer;

        public VaultState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            stateContext.IsJumpingAllowed = IsJumpingAllowed;
            
            stateContext.TargetHeight = stateContext.PlayerSettings.GeneralSettings.StandingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.CrouchSettings.CrouchingSpeed;
            
            _startPosition = stateContext.PlayerTransform.position + (Vector3.up * 0.15f);
            _previousPosition = stateContext.PlayerTransform.position;
            
            _duration = stateContext.PlayerSettings.VaultSettings.VaultDuration;

            _startSpeed = stateContext.HorizontalDesiredVelocity.magnitude;
            _angle = stateContext.VaultHandler.GetAdjustedAngle(_startSpeed);
            _vaultHorizontalDirection = stateContext.PlayerTransform.forward;
            
            _timer = 0f;
            
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.VaultSettings.VaultAnimationTrigger);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            _timer += Time.deltaTime;

            if (_timer >= _duration && stateContext.CharacterController.isGrounded)
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

        protected override bool IsJumpingAllowed()
        {
            return false;
        }

        protected override void HandleMovement()
        {
            Vector3 currentPosition = stateContext.VaultHandler.GetCurrentPosition(_startPosition, _startSpeed,
                _vaultHorizontalDirection, _angle, _timer);

            Vector3 velocity = (currentPosition - _previousPosition) / Time.deltaTime;
            stateContext.DesiredVelocity = velocity;
            
            _previousPosition = currentPosition;
        }
    }
}