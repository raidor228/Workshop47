using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class RunSettings : ConditionalMechanicSettings
    {
        [Tooltip("Name of the animation trigger used when the player starts running")]
        [field: SerializeField]
        public string RunAnimationTrigger { get; private set; } = "IsRunning";
        
        [Tooltip("The acceleration applied while running")] 
        [field: SerializeField, Range(0f, 300f)] 
        public float RunAcceleration { get; private set; } = 42f;
    }
}