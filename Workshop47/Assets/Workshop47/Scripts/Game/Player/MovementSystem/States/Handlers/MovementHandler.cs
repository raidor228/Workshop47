using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States.Handlers
{
    public class MovementHandler
    {
        private GeneralSettings GeneralSettings => _stateContext.PlayerSettings.GeneralSettings;
        
        private readonly MovementStateContext _stateContext;

        public MovementHandler(MovementStateContext stateContext)
        {
            _stateContext = stateContext;
        }

        public Vector3 ApplyFrictionToVelocity(Vector3 velocity, float friction)
        {
            float magnitude = velocity.magnitude;
            if (magnitude != 0f)
            {
                float velocityLoss = magnitude * friction * Time.deltaTime;
                return velocity * Mathf.Max(magnitude - velocityLoss, 0f) / magnitude;
            }

            return velocity;
        }
        
        public Vector3 AdjustVelocityToSlope(Vector3 playerPosition, Vector3 velocity)
        {
            if (Physics.Raycast(playerPosition, Vector3.down, out var hit, 2f))
            {
                var slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                var adjustedVelocity = slopeRotation * velocity;

                if (adjustedVelocity.y < 0)
                {
                    return adjustedVelocity;
                }
            }

            return velocity;
        }
        
        public void ApplyMovement()
        {
            _stateContext.CharacterController.Move(_stateContext.Velocity * Time.deltaTime);
            
            _stateContext.AnimatorHandler.SetFloat("MoveX", _stateContext.Velocity.x);
            _stateContext.AnimatorHandler.SetFloat("MoveY", _stateContext.Velocity.y);
            _stateContext.AnimatorHandler.SetFloat("MoveZ", _stateContext.Velocity.z);
        }

        public float ApplyGravity(float velocity, float fallFactor)
        {
            float y = velocity - fallFactor * Time.deltaTime;
            if (_stateContext.CharacterController.isGrounded)
            {
                y = -2f;
            }
            
            return Mathf.Max(y, GeneralSettings.MinVerticalVelocity);
        }

        public bool CanSprint()
        {
            Vector2 moveInput = _stateContext.MovementInput.MovementInput;
            if (moveInput == Vector2.zero || moveInput.y <= 0)
            {
                return false;
            }
            
            return true;
        }
        
        public float GetVelocityAfterHeadBounce(float verticalVelocity)
        {
            if (IsTouchingRoof() && _stateContext.DesiredVelocity.y > 0f)
            {
                return 0f;
            }
            else
            {
                return verticalVelocity;
            }
        }

        private bool IsTouchingRoof()
        {
            float height = _stateContext.CharacterController.height;
            float maxDistance = height + 0.1f;
            return Physics.Raycast(_stateContext.PlayerTransform.position, Vector3.up, 
                out _, maxDistance, GeneralSettings.RoofMask);
        }
    }
}