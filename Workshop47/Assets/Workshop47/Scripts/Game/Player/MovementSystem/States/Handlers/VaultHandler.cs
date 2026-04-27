using System;
using Melador.PlayerController.GameObjects;
using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States.Handlers
{
    public class VaultHandler
    {
        private VaultSettings VaultSettings => _stateContext.PlayerSettings.VaultSettings;
        
        private readonly MovementStateContext _stateContext;
        
        public VaultHandler(MovementStateContext stateContext)
        {
            _stateContext = stateContext;
        }

        public Vector3 GetCurrentPosition(Vector3 startPosition, float startSpeed, 
            Vector3 vaultDirection, float angle, float t)
        {
            if (startSpeed < VaultSettings.MinVelocityToLongVault)
            {
                startSpeed = VaultSettings.VelocityRange.x;
            }

            float horizontal = CalculateX(t, angle, startSpeed);
            float y = CalculateY(t, angle, startSpeed, 10f);

            Vector3 position = new Vector3(
                startPosition.x + horizontal * vaultDirection.x, 
                0f,
                startPosition.z + horizontal * vaultDirection.z);
            
            position.y = startPosition.y + y;
            
            return position;
        }
        
        public float GetAdjustedAngle(float startSpeed)
        {
            float t = Mathf.InverseLerp(VaultSettings.VelocityRange.x, 
                VaultSettings.VelocityRange.y, startSpeed);
            return Mathf.Lerp(VaultSettings.AngleRange.x, VaultSettings.AngleRange.y, t);
        }
        
        private float CalculateX(float t, float angle, float startSpeed)
        {
            float num1 = startSpeed * Mathf.Cos(angle * Mathf.Deg2Rad) * t;
            return num1;
        }
        
        private float CalculateY(float t, float angle, float startSpeed, float gravity)
        {
            float num1 = startSpeed * Mathf.Sin(angle * Mathf.Deg2Rad) * t;
            float num2 = gravity * Mathf.Pow(t, 2) / 2f;
            return num1 - num2;
        }
        
        public bool CanVault()
        {
            float movementAdjust = 
                Vector3.ClampMagnitude(_stateContext.HorizontalDesiredVelocity, 16f).magnitude / 12f;
            float checkDistance = _stateContext.CharacterController.radius * 1.2f + movementAdjust;

            if (!HasObjectInFront(checkDistance, VaultSettings.VaultLayer, 
                    out var obstacle))
            {
                return false;
            }

            if (obstacle.collider.TryGetComponent<Obstacle>(out var component))
            {
                if (_stateContext.PlayerTransform.position.y < component.ObstacleLevel)
                {
                    return false;
                }
            }
            else
            {
                throw new Exception($"Game object {obstacle.collider.name} no have obstacle component");
            }
            
            return true;
        }
        
        private bool HasObjectInFront(float distance, LayerMask layer, out RaycastHit obstacle)
        {
            Vector3 top = _stateContext.PlayerTransform.position + (Vector3.up * 0.25f);
            Vector3 bottom = top - (Vector3.up * 0.5f);

            RaycastHit[] hits = Physics.CapsuleCastAll(top, bottom, 0.25f,
                _stateContext.PlayerTransform.forward, distance, layer);

            if (hits.Length > 0)
            {
                obstacle = hits[0];
                return true;
            }

            obstacle = default;
            return false;
        }
    }
}