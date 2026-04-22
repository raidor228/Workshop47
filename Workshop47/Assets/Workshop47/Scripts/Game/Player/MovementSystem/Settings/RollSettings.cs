using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class RollSettings : ConditionalMechanicSettings
    {
        [Tooltip("Action to enter the rolling state")]
        [field: SerializeField]
        public InputActionReference RollAction { get; private set; }
        
        [Tooltip("Name of the animation trigger used when the player starts rolling")]
        [field: SerializeField]
        public string RollAnimationTrigger { get; private set; } = "IsRolling";
        
        [Tooltip("The height of the player while rolling")]
        [field: SerializeField]
        public float RollingHeight { get; private set; } = 1f;
        
        [Tooltip("The speed at which the player crouching to roll")]
        [field: SerializeField, Range(1f, 10f)]
        public float RollingSpeed { get; private set; } = 6f;
        
        [Tooltip("The force applied during a roll")]
        [field: SerializeField, Range(0f, 25f)]
        public float RollingForce { get; private set; } = 6f;
        
        [Tooltip("The duration of rolling")]
        [field: SerializeField, Range(0f, 2f)]
        public float RollingDuration { get; private set; } = 0.7f;
        
        [Tooltip("The cooldown time between rolls")]
        [field: SerializeField, Range(0f, 5f)]
        public float RollingCooldown { get; private set; } = 1f;
    }
}