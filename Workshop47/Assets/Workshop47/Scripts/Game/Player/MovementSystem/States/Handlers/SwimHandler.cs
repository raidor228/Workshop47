using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States.Handlers
{
    public class SwimHandler
    {
        public float CurrentWaterSurfaceLevel { get; set; }
        public bool IsSwimming { get; set; }
        public bool ShouldWaterJump { get; set; }
        
        private Vector3 _swimWalkDirection;
        private Vector3 _swimSprintDirection;

        private SwimSettings SwimSettings => _stateContext.PlayerSettings.SwimSettings;
        
        private readonly MovementStateContext _stateContext;

        public SwimHandler(MovementStateContext stateContext)
        {
            _stateContext = stateContext;
        }
        
        public Vector3 GetSwimDirection()
        {
            Vector2 movementInput = _stateContext.MovementInput.MovementInput;

            _swimWalkDirection = new Vector3(movementInput.x, 0f, movementInput.y);
            _swimWalkDirection = _stateContext.CameraTransformDirection(_swimWalkDirection);

            return _swimWalkDirection;
        }


        public void HandleSurfaceWaterJump()
        {
            bool isJumping = _stateContext.MovementInput.IsJumpButtonPressed;
            float playerHeightTrigger = 
                _stateContext.PlayerTransform.position.y + _stateContext.CharacterController.height / 1.6f;
            bool isAboveWater = playerHeightTrigger > CurrentWaterSurfaceLevel;

            if (isJumping && isAboveWater)
            {
                ShouldWaterJump = false;
            }

            if (isJumping && !ShouldWaterJump)
            {
                _stateContext.DesiredVelocity += Vector3.up * (10f * Time.deltaTime);
            }

            if (ShouldWaterJump)
            {
                _stateContext.DesiredVelocity += Vector3.up * (10f * Time.deltaTime);
            }
            else
            {
                var velocity = _stateContext.DesiredVelocity;
                velocity.y = Mathf.Lerp(velocity.y, 0f, 0.1f * Time.deltaTime);
                _stateContext.DesiredVelocity = velocity;
            }
        }
        
        public bool IsInWaterSurface()
        {
            if (!IsSwimming)
            {
                return false;
            }

            Vector3 playerPosition = _stateContext.PlayerTransform.position;
            float playerHeight = _stateContext.CharacterController.height;
            
            return playerPosition.y + playerHeight / 1.4f < CurrentWaterSurfaceLevel &&
                   playerPosition.y + playerHeight / 1.1f > CurrentWaterSurfaceLevel;
        }
        
        public bool IsUnderWater()
        {
            if (!IsSwimming)
            {
                return false;
            }
            
            return _stateContext.PlayerTransform.position.y + _stateContext.CharacterController.height / 1.1f 
                   < CurrentWaterSurfaceLevel;
        }
    }
}