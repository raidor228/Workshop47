using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class JumpSettings : ConditionalMechanicSettings
    {
        [Tooltip("Action to enter the jumping state")]
        [field: SerializeField]
        public InputActionReference JumpAction { get; private set; }
        
        [Tooltip("Name of the animation trigger used when the player starts jumping")]
        [field: SerializeField]
        public string JumpAnimationTrigger { get; private set; } = "IsJumping";
        
        [Tooltip("Enables or disables bunny hopping")]
        [field: SerializeField]
        public bool AutoBhop { get; private set; } = true;
        
        [Tooltip("The maximum number of jumps the player can perform")]
        [field: SerializeField, Min(1)]
        public int MaxJumps { get; private set; } = 1;
        
        [Tooltip("The height the player can reach when jumping")]
        [field: SerializeField, Min(0)]
        public float JumpHeight { get; private set; } = 1.5f;
        
        [Tooltip("The forward force applied to the player when jumping")]
        [field: SerializeField, Range(1f, 10f)]
        public float JumpForce { get; private set; } = 1.1f;
        
        [Tooltip("The amount of time the player can still jump after leaving the ground")]
        [field: SerializeField, Range(0.05f, 0.5f)]
        public float CoyoteTime { get; private set; } = 0.1f;
    }
}