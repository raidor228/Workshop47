using System;
using Melador.PlayerController.MovementController.Settings.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.Settings
{
    [Serializable]
    public class SwimSettings : ConditionalMechanicSettings
    {
        [Tooltip("Name of the animation trigger used when the player starts swimming")]
        [field: SerializeField]
        public string SwimAnimationTrigger { get; private set; } = "IsSwimming";
        
        [Tooltip("Name of the animation trigger used when the player starts sprint swimming")]
        [field: SerializeField]
        public string SprintSwimAnimationTrigger { get; private set; } = "IsSprintSwimming";
        
        [Tooltip("The speed of the player while swimming")]
        [field: SerializeField, Range(0f, 50f)]
        public float SwimSpeed { get; private set; } = 15f;

        [Tooltip("The speed of the player while sprinting underwater")]
        [field: SerializeField, Range(0f, 50f)]
        public float SprintSwimSpeed { get; private set; } = 15f;

        [Tooltip("The min angle to swim down")]
        [field: SerializeField, Range(-1f, 0f)]
        public float SwimDownThreshold { get; private set; } = -0.6f;
        
        [Tooltip("The buoyancy force")]
        [field: SerializeField, Range(0f, 20f)]
        public float BuoyancyForce { get; private set; } = 8f;
    }
}