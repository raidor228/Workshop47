using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States.Handlers
{
    public class SlopeHandler
    {
        public float SlideTransitionTimer { get; set; }

        private bool _isSlideBoostOnCooldown;
        private float _boostTimeStamp;
                
        private readonly float _slideTransitionDuration = 0.5f;
        private readonly float _steepSlopeResistance = 4f;

        private GeneralSettings GeneralSettings => _stateContext.PlayerSettings.GeneralSettings;
        private SlideSettings SlideSettings => _stateContext.PlayerSettings.SlideSettings;
        
        private readonly MovementStateContext _stateContext;
        
        public SlopeHandler(MovementStateContext stateContext)
        {
            _stateContext = stateContext;
        }
        
        public void AdjustVelocityOnSlope()
        {
            Vector3 tangent = GetSlopeTangent();
            Vector3 normal = GetSlopeNormal();

            Vector3.OrthoNormalize(ref normal, ref tangent);

            Vector3 velocityParallel = Vector3.Project(_stateContext.DesiredVelocity, tangent);
            Vector3 upTangent = -tangent;

            bool movingUp = Vector3.Dot(_stateContext.DesiredVelocity, upTangent) > 0;

            if (IsOnSteepSlope())
            {
                if (movingUp)
                {
                    velocityParallel = Vector3.Lerp(velocityParallel, Vector3.zero,
                        _steepSlopeResistance * Time.deltaTime);
                    _stateContext.DesiredVelocity = velocityParallel;
                }
                else
                {
                    velocityParallel = Vector3.Lerp(velocityParallel, tangent * GetDownSlopeAcceleration(), 
                        10f * Time.deltaTime);
                    _stateContext.DesiredVelocity = velocityParallel;
                }
            }
            else
            {
                _stateContext.DesiredVelocity = Vector3.RotateTowards(_stateContext.DesiredVelocity, tangent, 
                    2f * Time.deltaTime, 0f);
            }
        }

        public void ApplyBoostIfEligible()
        {
            if (_boostTimeStamp <= Time.time)
            {
                _isSlideBoostOnCooldown = false;
            }

            if (_stateContext.HorizontalDesiredVelocity.magnitude <= SlideSettings.BoostSpeedCap 
                && !_isSlideBoostOnCooldown)
            {
                _boostTimeStamp = Time.time + SlideSettings.BoostCooldown;
                _isSlideBoostOnCooldown = true;
            }
        }
        
        private float GetDownSlopeAcceleration()
        {
            SlideTransitionTimer += Time.deltaTime;
            float transitionProgress = Mathf.Clamp01(SlideTransitionTimer / _slideTransitionDuration);
            float increasedSpeed = Mathf.Lerp(5f, GeneralSettings.MaxVelocity, transitionProgress);
            float slopeFactor = 1 + GetSlopeAngle() / 90f;
            return increasedSpeed * slopeFactor;
        }
        
        private Vector3 GetSlopeNormal()
        {
            if (Physics.Raycast(_stateContext.PlayerTransform.position, Vector3.down, 
                    out RaycastHit hit, 2f, SlideSettings.SlidingLayer))
            {
                return hit.normal;
            }

            return Vector3.zero;
        }
        
        private Vector3 GetSlopeTangent()
        {
            Vector3 normal = GetSlopeNormal();
            return Vector3.ClampMagnitude(new Vector3(normal.x, -normal.y, normal.z), 1f);
        }

        private float GetSlopeAngle()
        {
            return Vector3.Angle(GetSlopeNormal(), Vector3.up);
        }

        public bool IsOnSlope()
        {
            return GetSlopeAngle() > 0;
        }

        public bool IsOnSteepSlope()
        {
            return GetSlopeAngle() > GeneralSettings.SlopeMaxAngle - 0.1f;
        }
    }
}