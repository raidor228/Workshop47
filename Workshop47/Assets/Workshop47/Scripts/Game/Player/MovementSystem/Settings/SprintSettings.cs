using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class SprintSettings : ConditionalMechanicSettings
    {
        [Tooltip("Action to enter the sprinting state")]
        [field: SerializeField]
        public InputActionReference SprintAction { get; private set; }
        
        [Tooltip("Name of the animation trigger used when the player starts sprinting")]
        [field: SerializeField]
        public string SprintAnimationTrigger { get; private set; } = "IsSprinting";
        
        [Tooltip("The acceleration applied while running")] 
        [field: SerializeField, Range(0f, 300f)] 
        public float SprintAcceleration { get; private set; } = 70f;
    }
}