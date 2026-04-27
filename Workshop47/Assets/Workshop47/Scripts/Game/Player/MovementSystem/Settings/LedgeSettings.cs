using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class LedgeSettings : ConditionalMechanicSettings
    {
        [Tooltip("The layers that the ledge detection will consider.")]
        [field: SerializeField]
        public LayerMask LedgeLayer { get; private set; }
        
        [Tooltip("Name of the animation trigger used when the player starts ledge grabbing")]
        [field: SerializeField]
        public string LedgeAnimationTrigger { get; private set; } = "IsLedgeGrabbing";
        
        [Tooltip("Name of the animation trigger used when the player starts jumping while ledge grabbing")]
        [field: SerializeField]
        public string LedgeJumpAnimationTrigger { get; private set; } = "IsLedgeJumping";
        
        [Tooltip("The height the player can reach when jumping on ledge")]
        [field: SerializeField, Min(0f)]
        public float LedgeJumpHeight { get; private set; } = 2.5f;
        
        [Tooltip("The side players speed when grab a ledge")]
        [field: SerializeField, Range(0f, 10f)]
        public float LedgeSpeed { get; private set; } = 1.2f;
        
        [Tooltip("The cooldown time before the player can regrab a same ledge")]
        [field: SerializeField, Range(0f, 5f)]
        public float SameLedgeCoolDown { get; private set; } = 0.5f;

        [Tooltip("The multiplier to the player's height to raycast the ledge")]
        [field: SerializeField, Range(0f, 1.5f)]
        public float LedgeGrabYPositionMultiplier { get; private set; } = 1.2f;
        
        [Tooltip("The vertical component of the unit vector to raycast the ledge")]
        [field: SerializeField, Range(-1f, 1f)]
        public float LedgeCastYDirection { get; private set; } = -0.5f;
    }
}