using UnityEngine;
using Workshop47.Scripts.Fsm;

namespace Melador.PlayerController.MovementController.States.Root
{
    public abstract class MovementBaseState : FsmState
    {
        protected readonly MovementStateContext stateContext;
        
        private readonly PlayerMovementFsm _fsm;
        
        protected MovementBaseState(MovementStateContext stateContext, PlayerMovementFsm fsm) : base(fsm)
        {
            this.stateContext = stateContext;
            _fsm = fsm;
        }

        public override void Enter()
        {
            stateContext.IsJumpingAllowed = IsJumpingAllowed;
        }

        public override void LogicUpdate()
        {
            UpdateCollider();
            HandleMovement();
            
            if (stateContext.PlayerSettings.JumpSettings.IsMechanicAllowed)
            {
                HandleJumping();
            }
            
            stateContext.LerpVelocity();
            stateContext.MovementHandler.ApplyMovement();
        }

        public override void OnTriggerEnter(Collider collider)
        {
            stateContext.CollisionResponder.OnTriggerEnter(collider);
        }

        public override void OnTriggerExit(Collider collider)
        {
            stateContext.CollisionResponder.OnTriggerExit(collider);
        }
        
        public override void OnControllerColliderHit(ControllerColliderHit hit)
        {
            stateContext.CollisionResponder.OnControllerColliderHit(hit);
            
            Vector3 normal = hit.normal;
            float verticalDot = Vector3.Dot(normal, Vector3.up);

            if (Mathf.Abs(verticalDot) < 0.5f)
            {
                if ((stateContext.PlayerSettings.GeneralSettings.WallBounceLayer.value & (1 << hit.gameObject.layer)) != 0)
                {
                    Vector3 velocityIntoWall = Vector3.Project(stateContext.DesiredVelocity, -normal);
                    stateContext.DesiredVelocity -= velocityIntoWall * (1f - stateContext.PlayerSettings.GeneralSettings.EnergyLossFactor);
                }
            }
        }
        
        protected virtual bool IsJumpingAllowed()
        {
            return false;
        }
        
        protected virtual void HandleMovement()
        {
        }
        
        protected virtual void HandleJumping()
        {
            if (!stateContext.IsJumpingAllowed())
            {
                return;
            }

            if (stateContext.CharacterController.isGrounded)
            {
                stateContext.JumpCount = 0;
            }

            if (stateContext.JumpCount >= stateContext.PlayerSettings.JumpSettings.MaxJumps)
            {
                return;
            }
            
            if (stateContext.PlayerSettings.JumpSettings.AutoBhop && stateContext.CharacterController.isGrounded)
            {
                if (stateContext.MovementInput.IsJumpButtonPressed)
                {
                    PerformManualJump();
                }
            }
            else if (stateContext.MovementInput.JumpAction.WasPerformedThisFrame())
            {
                PerformManualJump();
            }
        }
        
        protected void PerformManualJump()
        {
            stateContext.AnimatorHandler.SetTrigger(stateContext.PlayerSettings.JumpSettings.JumpAnimationTrigger);
            
            stateContext.JumpCount++;
            stateContext.HasJumped = true;
            
            float jumpVelocity = Mathf.Sqrt(-2f * stateContext.PlayerSettings.GeneralSettings.Gravity * 
                                            stateContext.PlayerSettings.JumpSettings.JumpHeight);
            
            stateContext.DesiredVelocity = new Vector3(stateContext.DesiredVelocity.x, jumpVelocity, stateContext.DesiredVelocity.z);
            
            stateContext.DesiredVelocity += stateContext.GetMoveDirection() * 
                                     (stateContext.PlayerSettings.JumpSettings.JumpForce * Time.deltaTime);
        }
        
        private void UpdateCollider()
        {
            float center = stateContext.TargetHeight / 2;

            stateContext.CharacterController.height = Mathf.Lerp(stateContext.CharacterController.height, 
                stateContext.TargetHeight, stateContext.UpdatingColliderSpeed * Time.deltaTime);
        
            stateContext.CharacterController.center = Vector3.Lerp(stateContext.CharacterController.center, 
                new Vector3(0f, center, 0f), stateContext.UpdatingColliderSpeed * Time.deltaTime);

            stateContext.PlayerRootTransform.localPosition = 
                stateContext.CharacterController.center +
                new Vector3(0f, stateContext.CharacterController.height / 2f - 0.2f, 0f);
        }
    }
}