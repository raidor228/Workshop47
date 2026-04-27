using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class LongJumpSettings : ConditionalMechanicSettings
    {
        [Tooltip("Name of the animation trigger used when the player starts long jumping")]
        [field: SerializeField]
        public string LongJumpAnimationTrigger { get; private set; } = "IsLongJumping";

        [field: SerializeField, Min(0)] 
        public float MinVelocityToLongJump { get; private set; } = 9f;
        
        [Tooltip("The height the player can reach when long jumping")]
        [field: SerializeField, Min(0)]
        public float LongJumpHeight { get; private set; } = 1.8f;
        
        [Tooltip("The forward force applied to the player when long jumping")]
        [field: SerializeField, Range(1f, 200f)]
        public float LongJumpForce { get; private set; } = 200f;
        
        [Tooltip("Cooldown time before the player can perform another long jump")]
        [field: SerializeField, Range(0f, 6f)]
        public float LongJumpCooldown { get; private set; } = 1.2f;
    }
}