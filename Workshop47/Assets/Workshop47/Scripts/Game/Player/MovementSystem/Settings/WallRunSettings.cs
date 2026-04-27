using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class WallRunSettings : ConditionalMechanicSettings
    {
        [Tooltip("Layer mask to identify walls")]
        [field: SerializeField]
        public LayerMask WallLayer { get; private set; }

        [Tooltip("Name of the animation trigger used when the player starts wall running")]
        [field: SerializeField]
        public string WallRunAnimationTrigger { get; private set; } = "IsWallRunning";
        
        [Tooltip("Name of the animation trigger used when the player starts jump off wall")]
        [field: SerializeField]
        public string WallRunJumpAnimationTrigger { get; private set; } = "IsWallRunJumping";
        
        [Tooltip("The acceleration applied while wall running")]
        [field: SerializeField, Range(1f, 300f)]
        public float WallAcceleration { get; private set; } = 100f;

        [Tooltip("The time window during which the player can kick off the wall")]
        [field: SerializeField, Range(0.05f, 1.2f)]
        public float WallKickWindow { get; private set; } = 0.2f;

        [Tooltip("The speed at which the player bounces off the wall")]
        [field: SerializeField, Range(0.1f, 1f)]
        public float WallBounceSpeed { get; private set; } = 0.25f;
        
        [Tooltip("The distance required for performing a wall jump")]
        [field: SerializeField, Range(1f, 20f)]
        public float WallJumpDistance { get; private set; } = 12f;
        
        [Tooltip("The duration of the wall run effect")]
        [field: SerializeField, Range(0f, 5f)]
        public float WallDuration { get; private set; } = 1f;
        
        [Tooltip("The cooldown time before the player can wall run again")]
        [field: SerializeField, Range(0f, 5f)]
        public float WallCooldown { get; private set; } = 0.1f;
        
        [Tooltip("The cooldown time when performing multiple wall runs on the same wall")]
        [field: SerializeField, Range(0f, 5f)]
        public float SameWallCooldown { get; private set; } = 0.5f;
        
        [Tooltip("Minimum vertical velocity to enter wall running state")]
        [field: SerializeField, Range(-20f, 20f)]
        public float MinVerticalVelocity { get; private set; } = -3f;
    }
}