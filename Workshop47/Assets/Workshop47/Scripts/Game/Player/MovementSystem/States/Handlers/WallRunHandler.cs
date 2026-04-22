using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States.Handlers
{
    public class WallRunHandler
    {
        public bool CanWallJump { get; set; }
        public float OffWallTimeStamp { get; set; }
        public Vector3 CurrentWallNormal { get; set; }
        public Vector3 PreviousWallNormal { get; set; }
        public Vector3 WallDirection { get; set; }

        private WallRunSettings WallRunSettings => _stateContext.PlayerSettings.WallRunSettings;
        private GeneralSettings GeneralSettings => _stateContext.PlayerSettings.GeneralSettings;
        
        private readonly MovementStateContext _stateContext;

        public WallRunHandler(MovementStateContext stateContext)
        {
            _stateContext = stateContext;
        }
        
        public float GetWallVelocityAngle()
        {
            return Vector3.Angle(GetWallNormal(), _stateContext.HorizontalDesiredVelocity);
        }

        public float GetDesiredWallNormalAngle()
        {
            return Vector3.Angle(GetWallNormal(), _stateContext.GetMoveDirection());
        }
        
        public Vector3 GetWallNormal()
        {
            return GetClosestWallAround().normal;
        }

        public float GetDistanceToWall()
        {
            float distance = GetClosestWallAround().distance;
            return distance == 0.0f ? 1000f : distance;
        }
        
        public Vector3 GetHorizontalParallel()
        {
            return Vector3.Cross(GetWallNormal(), Vector3.up).normalized;
        }
        
        public bool IsWallBeneath()
        {
            return Physics.Raycast(_stateContext.PlayerTransform.position, Vector3.down, 
                float.PositiveInfinity, WallRunSettings.WallLayer);
        }

        private RaycastHit GetClosestWallAround()
        {
            return _stateContext.GetClosestHitAround(WallRunSettings.WallLayer);
        }
        
        private bool IsSurfaceBanked()
        {
            float yNormal = GetWallNormal().y;
            if (Mathf.Abs(yNormal) < 0.001f)
            {
                return false;
            }
            
            return yNormal != 0.0f;
        }

        public bool ShouldMaintainWallRun()
        {
            return (_stateContext.CharacterController.isGrounded && IsSurfaceBanked()) || GetDistanceToWall() < 0.3f;
        }

        public Vector3 GetJumpDirection()
        {
            float velocityAngle = GetWallVelocityAngle();
            float desiredDirectionAngle = GetDesiredWallNormalAngle();
            float speed = _stateContext.HorizontalDesiredVelocity.magnitude;
            
            Vector3 wallDirection = WallDirection;
            Vector3 velocity = _stateContext.DesiredVelocity;

            if (Vector3.Angle(velocity, wallDirection) > Vector3.Angle(velocity, -wallDirection))
            {
                wallDirection = -wallDirection;
            }

            Vector3 wallJumpOffset = CurrentWallNormal * WallRunSettings.WallJumpDistance;

            if (desiredDirectionAngle > 130.0)
            {
                wallJumpOffset *= 0.75f;
            }

            if (desiredDirectionAngle == 0.0)
            {
                if (velocityAngle >= 140.0)
                {
                    float jumpDistance = WallRunSettings.WallJumpDistance * Mathf.Tan(velocityAngle * Mathf.Deg2Rad);
                    return wallDirection * -jumpDistance + wallJumpOffset;
                }

                float speedFactor = _stateContext.LinearMap(velocityAngle, new Vector2(110f, 140f), 
                    new Vector2(1f, 0.5f));
                
                return wallDirection * (speed * speedFactor * Time.deltaTime) + wallJumpOffset;
            }

            if (velocityAngle >= 140.0)
            {
                return wallDirection * Mathf.Max(speed * 0.5f, 10f) + wallJumpOffset;
            }

            if (speed < 23.0)
            {
                return wallDirection * Mathf.Min(speed + 6f, 25f) + wallJumpOffset;
            }

            float speedMultiplier = _stateContext.LinearMap(velocityAngle, new Vector2(110f, 140f), 
                new Vector2(1f, 0.5f));
            return wallDirection * ((speed * speedMultiplier + 2.0f) * Time.deltaTime) + wallJumpOffset;
        }
    }
}