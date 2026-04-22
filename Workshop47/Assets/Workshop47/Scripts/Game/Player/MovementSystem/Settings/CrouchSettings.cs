using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class CrouchSettings : ConditionalMechanicSettings
    {
        [Tooltip("Action to enter the crouching state")]
        [field: SerializeField]
        public InputActionReference CrouchAction { get; private set; }
        
        [Tooltip("Name of the animation trigger used when the player starts crouching")]
        [field: SerializeField]
        public string CrouchAnimationTrigger { get; private set; } = "IsCrouching";
        
        [Tooltip("The height of the player while crouching")]
        [field: SerializeField]
        public float CrouchingHeight { get; private set; } = 1f;
        
        [Tooltip("The speed at which the player crouches")]
        [field: SerializeField, Range(0f, 10f)]
        public float CrouchingSpeed { get; private set; } = 6f;

        [Tooltip("The acceleration applied to the player while crouching")]
        [field: SerializeField, Range(1f, 300f)]
        public float CrouchAcceleration { get; private set; } = 12f;
    }
}