using System;
using Melador.PlayerController.GameObjects;
using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States.Handlers
{
    public class LedgeHandler
    {
        public bool CanRegrabSameLedge { get; set; } = true;

        public RaycastHit CurrentLedgeHit => _currentLedgeHit;
        
        private Vector3 _leftLedgeDirection;
        private RaycastHit _currentLedgeHit;
        
        private LedgeSettings LedgeSettings => _stateContext.PlayerSettings.LedgeSettings;
        
        private readonly MovementStateContext _stateContext;
        
        public LedgeHandler(MovementStateContext stateContext)
        {
            _stateContext = stateContext;
        }

        public Vector3 GetLedgeHitPoint(bool originDirection)
        {
            Vector3 position = 
                _stateContext.PlayerTransform.position + 
                Vector3.up * (_stateContext.CharacterController.height * LedgeSettings.LedgeGrabYPositionMultiplier);

            Vector3 direction = _stateContext.PlayerTransform.TransformDirection(
                new Vector3(0, LedgeSettings.LedgeCastYDirection, 1));

            direction = originDirection ? direction : -_currentLedgeHit.normal;
            
            if (Physics.Raycast(position, direction, out _currentLedgeHit, 
                    _stateContext.CharacterController.radius * 1.5f + 0.125f, 
                    LedgeSettings.LedgeLayer))
            {
                if (_currentLedgeHit.collider.TryGetComponent<Ledge>(out var ledge))
                {
                    Vector3 point = _currentLedgeHit.point;
                    point.y = ledge.LedgeLevel;
                    
                    return point;
                }
                
                throw new Exception($"There is no Ledge component on GameObject with name " +
                                    $"{_currentLedgeHit.collider.name}");
            }

            return Vector3.zero;
        }
        
        public bool CanGrabLedge()
        {
            Vector3 hitPoint = GetLedgeHitPoint(true);
            if (hitPoint == Vector3.zero || !CanRegrabSameLedge)
            {
                return false;
            }

            if (_currentLedgeHit.normal == Vector3.up)
            {
                return false;
            }

            return true;
        }
    }
}