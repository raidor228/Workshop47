using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class WalkSettings : ConditionalMechanicSettings
    {
        [Tooltip("Action to enter the walking state")]
        [field: SerializeField]
        public InputActionReference WalkAction { get; private set; }
        
        [Tooltip("Name of the animation trigger used when the player starts walking")]
        [field: SerializeField]
        public string WalkAnimationTrigger { get; private set; } = "IsWalking";
        
        [Tooltip("The acceleration applied while walking")] 
        [field: SerializeField, Range(0f, 300f)] 
        public float WalkAcceleration { get; private set; } = 20f;
    }
}