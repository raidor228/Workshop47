using Melador.Utils;
using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;
using Workshop47.Scripts.Utils;

namespace Melador.PlayerController.MovementController.States
{
    public class LedgeGrabState : MovementBaseState
    {
        public LedgeGrabState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(stateContext, fsm)
        {
        }

        public override void Enter()
        {
            stateContext.IsJumpingAllowed = IsJumpingAllowed;
            
            stateContext.TargetHeight = stateContext.PlayerSettings.GeneralSettings.StandingHeight;
            stateContext.UpdatingColliderSpeed =
                stateContext.PlayerSettings.CrouchSettings.CrouchingSpeed;
            stateContext.DesiredVelocity = Vector3.zero;

            Vector3 hitPoint = stateContext.LedgeHandler.GetLedgeHitPoint(true);

            Vector3 playerPosition = stateContext.PlayerTransform.position;
            playerPosition.y = hitPoint.y - stateContext.CharacterController.height * 
                stateContext.PlayerSettings.LedgeSettings.LedgeGrabYPositionMultiplier;

            stateContext.PlayerTransform.position = playerPosition;
            
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.LedgeSettings.LedgeAnimationTrigger);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (stateContext.LedgeHandler.GetLedgeHitPoint(false) == Vector3.zero)
            {
                Fsm.SetState<AirState>();
                return;
            }

            Vector2 movementInput = stateContext.MovementInput.MovementInput;
            Vector3 cameraForward = stateContext.GetCameraForward();
            
            float angle = Vector3.SignedAngle(cameraForward, 
                stateContext.LedgeHandler.CurrentLedgeHit.normal, Vector3.up);
            if ((Mathf.Abs(angle) >= 90 && movementInput.y < 0) 
                || (Mathf.Abs(angle) < 90 && movementInput.y > 0)
                || stateContext.MovementInput.IsCrouchButtonPressed)
            {
                Fsm.SetState<AirState>();
                return;
            }
        }

        public override void Exit()
        {
            stateContext.LedgeHandler.CanRegrabSameLedge = false;
            Coroutines.Invoke(() => { stateContext.LedgeHandler.CanRegrabSameLedge = true; }, 
                stateContext.PlayerSettings.LedgeSettings.SameLedgeCoolDown);
        }

        protected override bool IsJumpingAllowed()
        {
            return true;
        }

        protected override void HandleJumping()
        {
            if (stateContext.MovementInput.IsJumpButtonPressed)
            {
                PerformLedgeJump();
            }
        }

        protected override void HandleMovement()
        {
            Vector2 movementInput = stateContext.MovementInput.MovementInput;
            
            Vector3 leftLedgeDirection = Vector3.Cross(Vector3.up, 
                stateContext.LedgeHandler.CurrentLedgeHit.normal);
            
            Vector3 moveDirection = leftLedgeDirection * movementInput.x;
            Vector3 cameraForward = stateContext.GetCameraForward();
            float angle = Vector3.SignedAngle(cameraForward, 
                stateContext.LedgeHandler.CurrentLedgeHit.normal, Vector3.up);
            
            if (Mathf.Abs(angle) >= 90)
            {
                moveDirection *= -1;
            }
            
            Vector3 moveVelocity = moveDirection * stateContext.PlayerSettings.LedgeSettings.LedgeSpeed;

            stateContext.DesiredVelocity = Vector3.Lerp(stateContext.DesiredVelocity, 
                moveVelocity, Time.deltaTime * 10f);
        }

        private void PerformLedgeJump()
        {
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.LedgeSettings.LedgeJumpAnimationTrigger);
            
            float jumpVelocity = Mathf.Sqrt(-2f * stateContext.PlayerSettings.GeneralSettings.Gravity * 
                                            stateContext.PlayerSettings.LedgeSettings.LedgeJumpHeight);
            stateContext.DesiredVelocity = new Vector3(0, jumpVelocity, 0);
            Fsm.SetState<AirState>();
        }
    }
}